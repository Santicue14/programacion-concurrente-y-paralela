/// <summary>
/// Cliente refleja la informaci�n de los compradores.
/// </summary>
public class ClienteDTO
{
    /// <summary>
    /// Identificador �nico del cliente.
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
    /// Tel�fono del cliente.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;
}