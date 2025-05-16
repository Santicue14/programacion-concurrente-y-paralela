namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Venta es el registro de la venta de un vehículo, asociado a un cliente y al vehículo vendido.
/// </summary>
public class Venta
{
    /// <summary>
    /// Identificador �nico de la venta.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador �nico del cliente.
    /// </summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Identificador �nico del vehículo.
    /// </summary>
    public int VehiculoId { get; set; }


    /// <summary>
    /// Fecha de la venta.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Total de la venta.
    /// </summary>
    public decimal Total { get; set; }
    
}
