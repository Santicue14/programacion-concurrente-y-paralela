/// <summary>
/// Cliente refleja la información de los compradores.
/// </summary>
public class ClienteDTO
{
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
    /// Teléfono del cliente.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;
}