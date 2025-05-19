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
    /// A�o del veh�culo.
    /// </summary>
    public int Anio { get; set; }

    /// <summary>
    /// Precio del veh�culo.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Cantidad de veh�culos disponibles en stock.
    /// </summary>
    public int Stock { get; set; } = 0;
}
