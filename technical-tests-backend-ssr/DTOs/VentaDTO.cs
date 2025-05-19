
/// <summary>
/// DTO para la venta de un vehículo.
/// </summary>
public class VentaDTO
{
    /// <summary>
    /// Identificador único de la venta.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Datos del cliente.
    /// </summary>
    public ClienteDTO? Cliente { get; set; }

    /// <summary>
    /// Datos del vehículo.
    /// </summary>
    public VehiculoDTO? Vehiculo { get; set; }

    /// <summary>
    /// Fecha de la venta.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Total de la venta.
    /// </summary>
    public decimal Total { get; set; }
    
}
