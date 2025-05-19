using System.ComponentModel.DataAnnotations;

namespace technical_tests_backend_ssr.Models.DTOs;

public class RegistroDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class TwoFactorDTO
{
    [Required]
    public string Codigo { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public bool RequiereTwoFactor { get; set; }
} 