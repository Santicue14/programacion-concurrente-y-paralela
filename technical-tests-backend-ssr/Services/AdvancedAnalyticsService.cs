using System.Collections.Concurrent;
using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Servicio de analytics que demuestra técnicas específicas de concurrencia y paralelismo
/// </summary>
public class AdvancedAnalyticsService
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVentaRepository _ventaRepository;

    // Semáforo normal para controlar acceso concurrente
    private readonly Semaphore _semaphore = new(1, 1);
    private readonly Dictionary<string, object> _cache = new();

    public AdvancedAnalyticsService(
        IVehiculoRepository vehiculoRepository,
        IClienteRepository clienteRepository,
        IVentaRepository ventaRepository)
    {
        _vehiculoRepository = vehiculoRepository;
        _clienteRepository = clienteRepository;
        _ventaRepository = ventaRepository;
    }

    /// <summary>
    /// Técnica 1: Task.WhenAll para ejecutar múltiples operaciones async en paralelo
    /// </summary>
    public async Task<DashboardStatsDTO> GetDashboardStatsWithTaskWhenAllAsync()
    {
        // Ejecutar todas las consultas a la base de datos en paralelo usando Task.WhenAll
        var vehiculosTask = _vehiculoRepository.GetAllAsync();
        var clientesTask = _clienteRepository.GetAllAsync();
        var ventasTask = _ventaRepository.GetAllAsync();

        // Esperar a que todas las tareas terminen en paralelo
        await Task.WhenAll(vehiculosTask, clientesTask, ventasTask);

        // Obtener los resultados
        var vehiculos = await vehiculosTask;
        var clientes = await clientesTask;
        var ventas = await ventasTask;

        // Usar PLINQ para procesar los datos en paralelo
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        var totalVehicles = vehiculos.AsParallel().Count();
        var totalClients = clientes.AsParallel().Count();
        var totalSales = ventas.AsParallel().Count();
        var totalRevenue = ventas.AsParallel().Sum(v => v.Total);
        var salesThisMonth = ventas.AsParallel()
            .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
            .Count();
        var revenueThisMonth = ventas.AsParallel()
            .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
            .Sum(v => v.Total);

        var averageTicket = totalSales > 0 ? totalRevenue / totalSales : 0;
        var conversionRate = totalClients > 0 ? Math.Round((decimal)totalSales / totalClients * 100, 2) : 0;

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
    /// Técnica 2: Parallel.Invoke para ejecutar cálculos CPU-intensivos en paralelo
    /// </summary>
    public async Task<DashboardStatsDTO> GetDashboardStatsWithParallelInvokeAsync()
    {
        // Obtener datos primero
        var vehiculosTask = _vehiculoRepository.GetAllAsync();
        var clientesTask = _clienteRepository.GetAllAsync();
        var ventasTask = _ventaRepository.GetAllAsync();

        await Task.WhenAll(vehiculosTask, clientesTask, ventasTask);

        var vehiculos = await vehiculosTask;
        var clientes = await clientesTask;
        var ventas = await ventasTask;

        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        // Variables para almacenar resultados
        int totalVehicles = 0, totalClients = 0, totalSales = 0, salesThisMonth = 0;
        decimal totalRevenue = 0, revenueThisMonth = 0;

        // Usar Parallel.Invoke para ejecutar todos los cálculos en paralelo
        Parallel.Invoke(
            () => totalVehicles = vehiculos.AsParallel().Count(),
            () => totalClients = clientes.AsParallel().Count(),
            () => totalSales = ventas.AsParallel().Count(),
            () => totalRevenue = ventas.AsParallel().Sum(v => v.Total),
            () => salesThisMonth = ventas.AsParallel()
                .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
                .Count(),
            () => revenueThisMonth = ventas.AsParallel()
                .Where(v => v.Fecha.Month == currentMonth && v.Fecha.Year == currentYear)
                .Sum(v => v.Total)
        );

        var averageTicket = totalSales > 0 ? totalRevenue / totalSales : 0;
        var conversionRate = totalClients > 0 ? Math.Round((decimal)totalSales / totalClients * 100, 2) : 0;

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
    /// Técnica 3: PLINQ para procesamiento paralelo de datos
    /// </summary>
    public async Task<List<string>> GetTopVehicleModelsWithPLinqAsync()
    {
        var vehiculos = await _vehiculoRepository.GetAllAsync();
        var ventas = await _ventaRepository.GetAllAsync();

        // Usar PLINQ para procesar y agrupar datos en paralelo
        var modelSales = vehiculos.AsParallel()
            .GroupBy(v => v.ModeloId)
            .Select(group => new
            {
                ModeloId = group.Key,
                VehicleIds = group.Select(v => v.Id).ToHashSet(),
                TotalSales = ventas.AsParallel()
                    .Where(v => group.Select(vh => vh.Id).Contains(v.VehiculoId))
                    .Sum(v => v.Total)
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(5)
            .Select(x => $"Modelo_{x.ModeloId}")
            .ToList();

        return modelSales;
    }

    /// <summary>
    /// Técnica 4: Semaphore normal para controlar acceso concurrente
    /// </summary>
    public async Task<int> GetTotalVehiclesWithSemaphoreAsync()
    {
        const string cacheKey = "total_vehicles";
        
        // Verificar cache primero (sin protección, solo lectura)
        if (_cache.ContainsKey(cacheKey))
        {
            return (int)_cache[cacheKey];
        }

        // Usar semáforo para controlar acceso concurrente
        _semaphore.WaitOne();
        try
        {
            // Verificar cache nuevamente por si otro hilo ya lo calculó
            if (_cache.ContainsKey(cacheKey))
            {
                return (int)_cache[cacheKey];
            }

            // Calcular valor usando PLINQ
            var vehiculos = await _vehiculoRepository.GetAllAsync();
            var count = vehiculos.AsParallel().Count();

            // Guardar en cache
            _cache[cacheKey] = count;

            return count;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Técnica combinada: Task.WhenAll + Parallel.Invoke + PLINQ
    /// </summary>
    public async Task<Dictionary<string, object>> GetCompleteAnalyticsAsync()
    {
        // Task.WhenAll para obtener datos en paralelo
        var vehiculosTask = _vehiculoRepository.GetAllAsync();
        var clientesTask = _clienteRepository.GetAllAsync();
        var ventasTask = _ventaRepository.GetAllAsync();

        await Task.WhenAll(vehiculosTask, clientesTask, ventasTask);

        var vehiculos = await vehiculosTask;
        var clientes = await clientesTask;
        var ventas = await ventasTask;

        // Variables para resultados
        int totalVehicles = 0, totalClients = 0;
        decimal averagePrice = 0;
        List<int> salesByYear = new();

        // Parallel.Invoke para cálculos paralelos
        Parallel.Invoke(
            () => totalVehicles = vehiculos.AsParallel().Count(),
            () => totalClients = clientes.AsParallel().Count(),
            () => averagePrice = vehiculos.AsParallel().Average(v => v.Precio),
            () => salesByYear = ventas.AsParallel()
                .GroupBy(v => v.Fecha.Year)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList()
        );

        return new Dictionary<string, object>
        {
            ["TotalVehicles"] = totalVehicles,
            ["TotalClients"] = totalClients,
            ["AveragePrice"] = averagePrice,
            ["SalesByYear"] = salesByYear
        };
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
} 