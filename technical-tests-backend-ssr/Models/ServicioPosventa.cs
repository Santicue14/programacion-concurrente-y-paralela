using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Servicio de posventa.
/// </summary>
public class ServicioPosventa   
{
    /// <summary>
    /// Identificador único del servicio de posventa.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador único del cliente asociado.
    /// </summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Identificador único del tipo de servicio.
    /// </summary>
    public int TipoServicioId { get; set; }

    /// <summary>
    /// Descripción del servicio.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de solicitud del servicio.
    /// </summary>
    public DateTime FechaSolicitud { get; set; }

    /// <summary>
    /// Fecha programada para el servicio.
    /// </summary>
    public DateTime? FechaProgramada { get; set; }

    /// <summary>
    /// Estado del servicio.
    /// </summary>
    public int Estado { get; set; }

    /// <summary>
    /// Observaciones asociadas al servicio.
    /// </summary>
    public string? Observaciones { get; set; }

    // Relaciones
    public Cliente? Cliente { get; set; }
    public TipoServicio? TipoServicio { get; set; }
}