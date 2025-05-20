/// <summary>
/// Cliente refleja la informaci�n de los compradores.
/// </summary>
public class VehiculoDTO
{
    /// <summary>
    /// Identificador �nico del veh�culo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la marca del veh�culo.
    /// </summary>
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del modelo del veh�culo.
    /// </summary>
    public string Modelo { get; set; } = string.Empty;

    /// <summary>
    /// ID del modelo del vehículo.
    /// </summary>
    public int ModeloId { get; set; }

    /// <summary>
    /// Ao del vehculo.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Precio del vehculo.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Cantidad de vehculos disponibles en stock.
    /// </summary>
    public int Stock { get; set; } = 0;
}
