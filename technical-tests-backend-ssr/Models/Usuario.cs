using System.ComponentModel.DataAnnotations;

namespace technical_tests_backend_ssr.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string? TwoFactorSecret { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? UltimoAcceso { get; set; }

    public bool EmailConfirmado { get; set; }

    public string? EmailConfirmationToken { get; set; }

    public DateTime? EmailConfirmationTokenExpiry { get; set; }
} 