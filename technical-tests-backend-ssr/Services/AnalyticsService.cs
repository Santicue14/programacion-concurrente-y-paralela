using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Servicio para manejar analytics y estadísticas del dashboard
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVentaRepository _ventaRepository;

    /// <summary>
    /// Constructor del servicio de analytics
    /// </summary>
    /// <param name="vehiculoRepository">Repositorio de vehículos</param>
    /// <param name="clienteRepository">Repositorio de clientes</param>
    /// <param name="ventaRepository">Repositorio de ventas</param>
    public AnalyticsService(
        IVehiculoRepository vehiculoRepository,
        IClienteRepository clienteRepository,
        IVentaRepository ventaRepository)
    {
        _vehiculoRepository = vehiculoRepository;
        _clienteRepository = clienteRepository;
        _ventaRepository = ventaRepository;
    }

    /// <summary>
    /// Obtiene las estadísticas principales del dashboard de forma paralela
    /// </summary>
    /// <param name="startDate">Fecha de inicio para filtrar (opcional)</param>
    /// <param name="endDate">Fecha de fin para filtrar (opcional)</param>
    /// <param name="vehicleBrand">Marca de vehículo para filtrar (opcional)</param>
    /// <param name="clientId">ID del cliente para filtrar (opcional)</param>
    /// <returns>Estadísticas del dashboard</returns>
    public async Task<DashboardStatsDTO> GetDashboardStatsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        string? vehicleBrand = null, 
        int? clientId = null)
    {
        // Ejecutar todas las consultas en paralelo usando Task.WhenAll
        var totalVehiclesTask = GetTotalVehiclesAsync();
        var totalClientsTask = GetTotalClientsAsync();
        var totalSalesTask = GetTotalSalesAsync(startDate, endDate, vehicleBrand, clientId);
        var totalRevenueTask = GetTotalRevenueAsync(startDate, endDate, vehicleBrand, clientId);
        var salesThisMonthTask = GetSalesThisMonthAsync();
        var revenueThisMonthTask = GetRevenueThisMonthAsync();

        // Esperar a que todas las tareas terminen
        await Task.WhenAll(totalVehiclesTask, totalClientsTask, totalSalesTask, 
                          totalRevenueTask, salesThisMonthTask, revenueThisMonthTask);

        // Obtener los resultados
        var totalVehicles = await totalVehiclesTask;
        var totalClients = await totalClientsTask;
        var totalSales = await totalSalesTask;
        var totalRevenue = await totalRevenueTask;
        var salesThisMonth = await salesThisMonthTask;
        var revenueThisMonth = await revenueThisMonthTask;

        // Calcular métricas derivadas
        var averageTicket = totalSales > 0 ? totalRevenue / totalSales : 0;
        var conversionRate = CalculateConversionRate(totalSales, totalClients);

        return new DashboardStatsDTO
        {
            TotalVehicles = totalVehicles,
            TotalClients = totalClients,
            TotalSales = totalSales,
            TotalRevenue = totalRevenue,
            SalesThisMonth = salesThisMonth,
            RevenueThisMonth = revenueThisMonth,
            AverageTicket = averageTicket,
            ConversionRate = conversionRate
        };
    }

    /// <summary>
    /// Obtiene el número total de vehículos en catálogo usando PLINQ
    /// </summary>
    /// <returns>Cantidad de vehículos disponibles</returns>
    public async Task<int> GetTotalVehiclesAsync()
    {
        var vehiculos = await _vehiculoRepository.GetAllAsync();
        
        // Usar PLINQ para contar en paralelo (útil si hay muchos vehículos)
        return vehiculos.AsParallel().Count();
    }

    /// <summary>
    /// Obtiene el número total de clientes usando PLINQ
    /// </summary>
    /// <returns>Cantidad de clientes</returns>
    private async Task<int> GetTotalClientsAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        
        // Usar PLINQ para procesamiento paralelo
        return clientes.AsParallel().Count();
    }

    /// <summary>
    /// Obtiene el número total de ventas con filtros usando PLINQ
    /// </summary>
    private async Task<int> GetTotalSalesAsync(DateTime? startDate, DateTime? endDate, string? vehicleBrand, int? clientId)
    {
        var ventas = await _ventaRepository.GetAllAsync();
        
        // Usar PLINQ para filtrado y conteo paralelo
        return ventas.AsParallel()
            .Where(v => (startDate == null || v.Fecha >= startDate) &&
                       (endDate == null || v.Fecha <= endDate) &&
                       (clientId == null || v.ClienteId == clientId))
            // TODO: Agregar filtro por marca cuando esté disponible en el modelo
            .Count();
    }

    /// <summary>
    /// Obtiene el total de ingresos con filtros usando PLINQ
    /// </summary>
    private async Task<decimal> GetTotalRevenueAsync(DateTime? startDate, DateTime? endDate, string? vehicleBrand, int? clientId)
    {
        var ventas = await _ventaRepository.GetAllAsync();
        
        // Usar PLINQ para filtrado y suma paralela
        return ventas.AsParallel()
            .Where(v => (startDate == null || v.Fecha >= startDate) &&
                       (endDate == null || v.Fecha <= endDate) &&
                       (clientId == null || v.ClienteId == clientId))
            .Sum(v => v.Total);
    }

    /// <summary>
    /// Obtiene las ventas del mes actual
    /// </summary>
    private async Task<int> GetSalesThisMonthAsync()
    {
        var ventas = await _ventaRepository.GetAllAsync();
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        
        // Usar PLINQ para filtrado paralelo
        return ventas.AsParallel()
            .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
            .Count();
    }

    /// <summary>
    /// Obtiene los ingresos del mes actual
    /// </summary>
    private async Task<decimal> GetRevenueThisMonthAsync()
    {
        var ventas = await _ventaRepository.GetAllAsync();
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        
        // Usar PLINQ para filtrado y suma paralela
        return ventas.AsParallel()
            .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
            .Sum(v => v.Total);
    }

    /// <summary>
    /// Calcula la tasa de conversión
    /// </summary>
    private decimal CalculateConversionRate(int totalSales, int totalClients)
    {
        if (totalClients == 0) return 0;
        return Math.Round((decimal)totalSales / totalClients * 100, 2);
    }
} 