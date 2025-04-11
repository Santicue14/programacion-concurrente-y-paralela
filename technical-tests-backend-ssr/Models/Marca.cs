namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Veh�culo es el producto principal de nuestra Concesionaria.
/// </summary>
public class Marca
{
    /// <summary>
    /// Identificador �nico de la marca.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Marca del veh�culo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Modelo> Modelos { get; set; }
}