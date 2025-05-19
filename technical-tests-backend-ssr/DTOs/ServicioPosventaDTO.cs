namespace technical_tests_backend_ssr.Models.DTOs;

public class ServicioPosventaDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string TipoServicio { get; set; } = string.Empty; // "MANTENIMIENTO", "GARANTIA", "RECLAMO"
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public DateTime? FechaProgramada { get; set; }
    public string Estado { get; set; } = string.Empty; // "PENDIENTE", "EN_PROCESO", "COMPLETADO", "CANCELADO"
    public string? Observaciones { get; set; }
} 