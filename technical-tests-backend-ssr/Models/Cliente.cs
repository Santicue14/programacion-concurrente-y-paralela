namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Cliente es aquel que adquiere nuestros autos.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Identificador único del cliente.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del cliente.
    /// </summary>
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Email del cliente.
    /// </summary>
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// Telefono del cliente.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

}