namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Interfaz para el servicio de analytics del dashboard
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Obtiene las estadísticas principales del dashboard
    /// </summary>
    /// <param name="startDate">Fecha de inicio para filtrar (opcional)</param>
    /// <param name="endDate">Fecha de fin para filtrar (opcional)</param>
    /// <param name="vehicleBrand">Marca de vehículo para filtrar (opcional)</param>
    /// <param name="clientId">ID del cliente para filtrar (opcional)</param>
    /// <returns>Estadísticas del dashboard</returns>
    Task<DashboardStatsDTO> GetDashboardStatsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        string? vehicleBrand = null, 
        int? clientId = null);

    /// <summary>
    /// Obtiene el número total de vehículos en catálogo
    /// </summary>
    /// <returns>Cantidad de vehículos disponibles</returns>
    Task<int> GetTotalVehiclesAsync();
} 