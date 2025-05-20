using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.DTOs;

namespace technical_tests_backend_ssr.Services;

public interface IAuthService
{
    Task<AuthResponseDTO> RegistrarAsync(RegistroDTO registro);
    Task<AuthResponseDTO> LoginAsync(LoginDTO login);
    Task<AuthResponseDTO> VerificarTwoFactorAsync(string email, string codigo);
    Task<bool> ConfirmarEmailAsync(string token);
    Task<bool> HabilitarTwoFactorAsync(int usuarioId);
    Task<bool> DeshabilitarTwoFactorAsync(int usuarioId);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<AuthResponseDTO> RegistrarAsync(RegistroDTO registro)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == registro.Email))
        {
            return new AuthResponseDTO { Success = false, Message = "El email ya está registrado" };
        }

        var usuario = new Usuario
        {
            Email = registro.Email,
            PasswordHash = HashPassword(registro.Password),
            EmailConfirmationToken = GenerateToken(),
            EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24)
        };

        _context.Usuarios.Add(usuario);

        var tareaCrear = Task.Run(async () => await _context.SaveChangesAsync());

        // Enviar email de confirmación
        var tareaEnviarEmail = Task.Run(async () => await _emailService.EnviarEmailConfirmacionAsync(usuario.Email, usuario.EmailConfirmationToken!));

        await Task.WhenAll(tareaCrear, tareaEnviarEmail);
        // Las hacemos como una función anónima para que se ejecute en paralelo
       
        return new AuthResponseDTO { Success = true, Message = "Registro exitoso. Por favor, confirma tu email." };
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO login)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (usuario == null || !VerifyPassword(login.Password, usuario.PasswordHash))
        {
            return new AuthResponseDTO { Success = false, Message = "Credenciales inválidas" };
        }

        if (!usuario.EmailConfirmado)
        {
            return new AuthResponseDTO { Success = false, Message = "Por favor, confirma tu email antes de iniciar sesión" };
        }

        usuario.UltimoAcceso = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        if (usuario.TwoFactorEnabled)
        {
            var codigo = GenerateTwoFactorCode();
            usuario.TwoFactorSecret = codigo;
            await _context.SaveChangesAsync();
            await _emailService.EnviarCodigoTwoFactorAsync(usuario.Email, codigo);
            return new AuthResponseDTO { Success = true, RequiereTwoFactor = true, Message = "Se ha enviado un código de verificación a tu email" };
        }

        var token = GenerateJwtToken(usuario);
        return new AuthResponseDTO { Success = true, Token = token };
    }

    public async Task<AuthResponseDTO> VerificarTwoFactorAsync(string email, string codigo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null || usuario.TwoFactorSecret != codigo)
        {
            return new AuthResponseDTO { Success = false, Message = "Código inválido" };
        }

        usuario.TwoFactorSecret = null;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(usuario);
        return new AuthResponseDTO { Success = true, Token = token };
    }

    public async Task<bool> ConfirmarEmailAsync(string token)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);
        if (usuario == null || usuario.EmailConfirmationTokenExpiry < DateTime.UtcNow)
        {
            return false;
        }
        usuario.EmailConfirmado = true;
        usuario.EmailConfirmationToken = null;
        usuario.EmailConfirmationTokenExpiry = null;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HabilitarTwoFactorAsync(int usuarioId)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null) return false;

        usuario.TwoFactorEnabled = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeshabilitarTwoFactorAsync(int usuarioId)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null) return false;

        usuario.TwoFactorEnabled = false;
        usuario.TwoFactorSecret = null;
        await _context.SaveChangesAsync();
        return true;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    private string GenerateToken()
    {        
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    private string GenerateTwoFactorCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var keyString = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyString))
        {
            throw new InvalidOperationException("JWT Key no está configurada en appsettings.json");
        }

        // Aseguramos que la clave tenga al menos 16 caracteres (128 bits)
        if (keyString.Length < 16)
        {
            throw new InvalidOperationException("JWT Key debe tener al menos 16 caracteres");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 