/// <summary>
/// Cliente refleja la información de los compradores.
/// </summary>
public class VehiculoDTO
{
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
