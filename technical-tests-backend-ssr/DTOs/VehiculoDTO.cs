/// <summary>
/// Cliente refleja la informaci�n de los compradores.
/// </summary>
public class VehiculoDTO
{
    public int Id { get; set; }
    /// <summary>
    /// Marca del veh�culo.
    /// </summary>
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Modelo del veh�culo.
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
    /// Stock del veh�culo.
    /// </summary>
    public int Stock { get; set; } = 0;
}
