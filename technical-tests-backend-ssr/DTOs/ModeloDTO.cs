namespace technical_tests_backend_ssr.Models.DTOs;

public class ModeloDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public string MarcaNombre { get; set; } = string.Empty;
} 