namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Vehículo es el producto principal de nuestra Concesionaria.
/// </summary>
public class Vehiculo
{
    /// <summary>
    /// Identificador único del vehículo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Modelo del vehículo.
    /// </summary>
    public int ModeloId { get; set; }
    public Modelo Modelo { get; set; }


    /// <summary>
    /// Año del vehículo.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Precio del vehículo.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Stock del vehículo.
    /// </summary>
    public int Stock { get; set; } = 0;
}