namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Producto refleja la esencia de la boutique: artefactos exclusivos para autos.
/// </summary>
public class Producto
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Precio del producto.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// sotck del producto.
    /// </summary>
    public int Stock { get; set; }
}