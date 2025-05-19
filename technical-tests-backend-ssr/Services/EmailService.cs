using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace technical_tests_backend_ssr.Services;

public interface IEmailService
{
    Task EnviarEmailConfirmacionAsync(string email, string token);
    Task EnviarCodigoTwoFactorAsync(string email, string codigo);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpServer = _configuration["Email:SmtpServer"]!;
        _smtpPort = int.Parse(_configuration["Email:SmtpPort"]!);
        _smtpUsername = _configuration["Email:Username"]!;
        _smtpPassword = _configuration["Email:Password"]!;
        _fromEmail = _configuration["Email:FromEmail"]!;
    }

    public async Task EnviarEmailConfirmacionAsync(string email, string token)
    {
        var subject = "Confirma tu cuenta";
        var body = $@"
            <h2>Bienvenido a nuestra plataforma</h2>
            <p>Por favor, confirma tu cuenta haciendo clic en el siguiente enlace:</p>
            <p><a href='{_configuration["AppUrl"]}/confirmar-email?token={token}'>Confirmar Email</a></p>
            <p>Este enlace expirará en 24 horas.</p>";

        await EnviarEmailAsync(email, subject, body);
    }

    public async Task EnviarCodigoTwoFactorAsync(string email, string codigo)
    {
        var subject = "Código de verificación";
        var body = $@"
            <h2>Código de verificación</h2>
            <p>Tu código de verificación es: <strong>{codigo}</strong></p>
            <p>Este código expirará en 5 minutos.</p>";

        await EnviarEmailAsync(email, subject, body);
    }

    private async Task EnviarEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword)
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(to);

        await client.SendMailAsync(message);
    }
} 