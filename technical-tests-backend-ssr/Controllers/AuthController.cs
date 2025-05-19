using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    public async Task<ActionResult<AuthResponseDTO>> Registro(RegistroDTO registro)
    {
        var resultado = await _authService.RegistrarAsync(registro);
        if (!resultado.Success)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO login)
    {
        var resultado = await _authService.LoginAsync(login);
        if (!resultado.Success)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [HttpPost("verificar-2fa")]
    public async Task<ActionResult<AuthResponseDTO>> VerificarTwoFactor([FromBody] TwoFactorDTO twoFactor)
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new AuthResponseDTO { Success = false, Message = "Email no encontrado" });
        }

        var resultado = await _authService.VerificarTwoFactorAsync(email, twoFactor.Codigo);
        if (!resultado.Success)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [HttpGet("confirmar-email")]
    public async Task<IActionResult> ConfirmarEmail([FromQuery] string token)
    {
        var resultado = await _authService.ConfirmarEmailAsync(token);
        if (!resultado)
        {
            return BadRequest(new { Message = "Token inválido o expirado" });
        }
        return Ok(new { Message = "Email confirmado exitosamente" });
    }

    [Authorize]
    [HttpPost("habilitar-2fa")]
    public async Task<IActionResult> HabilitarTwoFactor()
    {
        var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
        var resultado = await _authService.HabilitarTwoFactorAsync(usuarioId);
        if (!resultado)
        {
            return BadRequest(new { Message = "No se pudo habilitar 2FA" });
        }
        return Ok(new { Message = "2FA habilitado exitosamente" });
    }

    [Authorize]
    [HttpPost("deshabilitar-2fa")]
    public async Task<IActionResult> DeshabilitarTwoFactor()
    {
        var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
        var resultado = await _authService.DeshabilitarTwoFactorAsync(usuarioId);
        if (!resultado)
        {
            return BadRequest(new { Message = "No se pudo deshabilitar 2FA" });
        }
        return Ok(new { Message = "2FA deshabilitado exitosamente" });
    }
} 