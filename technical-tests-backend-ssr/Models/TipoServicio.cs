namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Modelo que representa un tipo de servicio de posventa.
/// </summary>
public class TipoServicio
{
    /// <summary>
    /// Identificador único del tipo de servicio.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del tipo de servicio.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del tipo de servicio.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el tipo de servicio está activo.
    /// </summary>
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del tipo de servicio.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Relaciones
    public ICollection<ServicioPosventa>? ServiciosPosventa { get; set; }
} 