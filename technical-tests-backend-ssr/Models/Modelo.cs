namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Veh�culo es el producto principal de nuestra Concesionaria.
/// </summary>
public class Modelo
{
    /// <summary>
    /// Identificador �nico del modelo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Modelo del veh�culo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Marca del modelo.
    /// </summary>
    public int MarcaId { get; set; }
    /// <summary>
    /// Marca asociada a este modelo.
    /// </summary>
    public Marca? Marca { get; set; } 

    /// <summary>
    /// Veh�culos asociados a este modelo.
    /// </summary>
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}