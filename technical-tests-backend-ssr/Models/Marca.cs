namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Vehículo es el producto principal de nuestra Concesionaria.
/// </summary>
public class Marca
{
    /// <summary>
    /// Identificador único de la marca.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Marca del vehículo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Modelo> Modelos { get; set; }
}