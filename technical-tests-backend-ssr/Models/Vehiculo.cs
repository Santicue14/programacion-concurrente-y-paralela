namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Veh�culo es el producto principal de nuestra Concesionaria.
/// </summary>
public class Vehiculo
{
    /// <summary>
    /// Identificador �nico del veh�culo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Modelo del veh�culo.
    /// </summary>
    public int ModeloId { get; set; }
    /// <summary>
    /// Modelo del veh�culo.
    /// </summary>
    public Modelo? Modelo { get; set; }


    /// <summary>
    /// A�o del veh�culo.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Precio del veh�culo.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Stock del veh�culo.
    /// </summary>
    public int Stock { get; set; } = 0;
}