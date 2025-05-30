/// <summary>
/// DTO para las estadísticas principales del dashboard
/// </summary>
public class DashboardStatsDTO
{
    /// <summary>
    /// Número total de ventas registradas
    /// </summary>
    public int TotalSales { get; set; }

    /// <summary>
    /// Ingresos totales generados
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Número total de clientes registrados
    /// </summary>
    public int TotalClients { get; set; }

    /// <summary>
    /// Número total de vehículos en catálogo
    /// </summary>
    public int TotalVehicles { get; set; }

    /// <summary>
    /// Número de ventas del mes actual
    /// </summary>
    public int SalesThisMonth { get; set; }

    /// <summary>
    /// Ingresos del mes actual
    /// </summary>
    public decimal RevenueThisMonth { get; set; }

} 