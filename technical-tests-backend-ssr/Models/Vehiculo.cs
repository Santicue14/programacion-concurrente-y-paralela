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
    /// Marca del vehículo.
    /// </summary>
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Modelo del vehículo.
    /// </summary>
    public string Modelo { get; set; } = string.Empty;

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