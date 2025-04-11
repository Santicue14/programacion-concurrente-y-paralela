/// <summary>
/// Cliente refleja la información de los compradores.
/// </summary>
public class VehiculoDTO
{
    /// <summary>
    /// Identificador único del vehículo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la marca del vehículo.
    /// </summary>
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del modelo del vehículo.
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
    /// Cantidad de vehículos disponibles en stock.
    /// </summary>
    public int Stock { get; set; } = 0;
}
