using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using technical_tests_backend_ssr.Services;


namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Service class for managing Venta entities.
/// </summary>
public class VentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly INotificationService _notificationService;
    private readonly VehiculoService _vehiculoService;
    /// <summary>
    /// Constructor for VentaService.
    /// </summary>
    /// <param name="ventaRepository"></param>
    /// <param name="clienteRepository"></param>
    /// <param name="vehiculoRepository"></param>
    /// <param name="notificationService"></param>
    /// <param name="vehiculoService"></param>
    public VentaService(
        IVentaRepository ventaRepository, 
        IClienteRepository clienteRepository, 
        IVehiculoRepository vehiculoRepository,
        INotificationService notificationService,
        VehiculoService vehiculoService
        )
    {
        _ventaRepository = ventaRepository;
        _clienteRepository = clienteRepository;
        _vehiculoRepository = vehiculoRepository;
        _vehiculoService = vehiculoService;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Retrieves all ventas asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<VentaDTO>> GetAllVentasAsync()
    {
        //Cronometr
        var cronometro = Stopwatch.StartNew();
        var ventas = await _ventaRepository.GetAllAsync();
        var clientes = await _clienteRepository.GetAllAsync();

        var vehiculosDetalles = await _vehiculoService.GetAllAsync();


        //Mapear clientes
        var clientesMapeados = clientes.AsParallel().Select(cliente => new ClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Email = cliente.Email,
            Telefono = cliente.Telefono
        }).ToList();

        var ventasMapeadas = ventas
        .AsParallel()
        .Select(venta => new VentaDTO
        {
            Id = venta.Id,
            Cliente = clientesMapeados.FirstOrDefault(c => c.Id == venta.ClienteId),
            Vehiculo = vehiculosDetalles.FirstOrDefault(v => v.Id == venta.VehiculoId),
            Fecha = venta.Fecha,
            Total = venta.Total
        }).ToList();
        cronometro.Stop();
        Console.WriteLine($"Tiempo de ejecución de consulta de ventas: {cronometro.ElapsedMilliseconds} ms");
        return ventasMapeadas;
    }    

    /// <summary>
    /// Retrieves a venta by ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Venta?> GetVentaByIdAsync(int id)
    {   
        var cronometro = Stopwatch.StartNew();
        var venta = await _ventaRepository.GetByIdAsync(id);
        if (venta == null) return null;

        var cliente = await _clienteRepository.GetByIdAsync(venta.ClienteId);
        var vehiculo = await _vehiculoRepository.GetByIdAsync(venta.VehiculoId);

        venta.Cliente = cliente;
        venta.Vehiculo = vehiculo;

        cronometro.Stop();
        Console.WriteLine($"Tiempo de ejecución de consulta de venta por id: {cronometro.ElapsedMilliseconds} ms");
        return venta;
    }

    /// <summary>
    /// Adds a new venta asynchronously.
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    public async Task<Venta> AddVentaAsync(Venta venta)
    {
        
        var cronometro = Stopwatch.StartNew();
        
        // Validar que el vehiculo exista
        var vehiculo = await _vehiculoRepository.GetByIdAsync(venta.VehiculoId);
        if (vehiculo == null)
        {
            throw new Exception("El vehiculo no existe.");
        }

        // Validar que el cliente exista
        var cliente = await _clienteRepository.GetByIdAsync(venta.ClienteId);
        if (cliente == null)
        {
            throw new Exception("El cliente no existe.");
        }

        // Validar que el vehiculo tenga stock
        if (vehiculo.Stock <= 0)
        {
            throw new Exception("El vehiculo no tiene stock.");
        }



        // Crear las tareas para ejecutar en paralelo
        // Actualizar el stock del vehiculo
        var tareaStock = Task.Run(async () => {
            vehiculo.Stock -= 1;
            await _vehiculoRepository.UpdateStockAsync(vehiculo.Id);
        });


        // Las hacemos como una función anónima para que se ejecute en paralelo
        var tareaVenta = Task.Run(async () => await _ventaRepository.AddAsync(venta)); 
        var tareaNotificacion = Task.Run(async () => await _notificationService.SendSaleNotificationAsync(venta)); 

        // Esperar a que ambas tareas terminen
        await Task.WhenAll(tareaVenta, tareaNotificacion, tareaStock);

        venta.Vehiculo = vehiculo;
        venta.Cliente = cliente;
        cronometro.Stop();
        Console.WriteLine($"Tiempo de ejecución de creación de venta: {cronometro.ElapsedMilliseconds} ms");
        
        return venta;
    }

    /// <summary>
    /// Updates an existing venta asynchronously.
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    public async Task<Venta> UpdateVentaAsync(Venta venta)
    {
        await _ventaRepository.UpdateAsync(venta);
        return venta;
    }

    /// <summary>
    /// Deletes a venta by ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteVentaAsync(int id)
    {
        var existingVenta = await _ventaRepository.GetByIdAsync(id);
        if (existingVenta == null) return false;

        await _ventaRepository.DeleteAsync(id);
        return true;
    }


    /// <summary>
    /// Obtiene el número total de ventas
    /// </summary>
    /// <returns>Cantidad de ventas</returns>
    public async Task<int> GetTotalSalesAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        return ventas.Count();
    }

    /// <summary>
    /// Obtiene el total de ingresos
    /// </summary>
    /// <returns>Total de ingresos</returns>
    public async Task<decimal> GetTotalRevenueAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        return ventas.AsParallel().Sum(v => v.Total);
    }

    /// <summary>
    /// Obtiene el total de ventas del mes actual
    /// </summary>
    /// <returns>Total de ventas del mes actual</returns>
    public async Task<int> GetSalesThisMonthAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        return ventas.AsParallel().Where(v => v.Fecha.Month == DateTime.Now.Month && v.Fecha.Year == DateTime.Now.Year).Count();
    }

    /// <summary>
    /// Obtiene el total de ingresos del mes actual
    /// </summary>
    /// <returns>Total de ingresos del mes actual</returns>
    public async Task<decimal> GetRevenueThisMonthAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        return ventas.AsParallel().Where(v => v.Fecha.Month == DateTime.Now.Month && v.Fecha.Year == DateTime.Now.Year).Sum(v => v.Total);
    }

    /// <summary>
    /// Obtiene las 5 marcas más vendidas con su respectivo total de ventas y porcentaje de ventas
    /// </summary>
    /// <returns>Ventas por marca</returns>
    public async Task<IEnumerable<Object>> GetSalesByBrandAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        var totalVentas = await this.GetTotalRevenueAsync();
        var vehiculos = await _vehiculoService.GetAllAsync();

        var marcas = vehiculos.AsParallel().Select(v => v.Marca).Distinct().ToList();

        var ventasPorMarca = marcas
        .AsParallel()
        .Select(m => new
        {
            Marca = m,
            Total = ventas.AsParallel().Where(v => v.Vehiculo.Marca == m).Sum(v => v.Total),
            Porcentaje = (ventas.AsParallel().Where(v => v.Vehiculo.Marca == m).Sum(v => v.Total) / totalVentas) * 100
        })
        .OrderByDescending(v => v.Total)
        .Take(5)
        .ToList();

        return ventasPorMarca;
    }

    /// <summary>
    /// Obtiene las 10 modelos más vendidos con su respectivo total de ventas y porcentaje de ventas
    /// </summary>
    /// <returns>Ventas por modelo</returns>
    public async Task<IEnumerable<Object>> GetSalesByModelAsync()
    {
        var ventas = await this.GetAllVentasAsync();

        var vehiculos = await _vehiculoService.GetAllAsync();
        var modelos = vehiculos.AsParallel().Select(v => v.Modelo).Distinct().ToList();

        var totalVentas = await this.GetTotalRevenueAsync();
        var ventasPorModelo = ventas.AsParallel().GroupBy(v => v.Vehiculo.Modelo).Select(g => new {
            Modelo = g.Key,
            Total = g.Sum(v => v.Total),
            Porcentaje = (g.Sum(v => v.Total) / totalVentas) * 100
        }).ToList();

        var ventasConMarcaYModelo = ventas
            .AsParallel()
            .Select(v => new
            {
                Marca = v.Vehiculo.Marca,
                Modelo = modelos.FirstOrDefault(m => m == v.Vehiculo.Modelo),
                CantidadVentas = ventas.AsParallel().Where(v => v.Vehiculo.Modelo == v.Vehiculo.Modelo).Count(),
                Porcentaje = (v.Total / totalVentas) * 100
        })
        .OrderByDescending(v => v.CantidadVentas)
        .Take(10)
        .ToList();
        return ventasConMarcaYModelo;
    }

    /// <summary>
    /// Obtiene las últimas 5 ventas
    /// </summary>
    /// <returns>Últimas 5 ventas</returns>
    public async Task<IEnumerable<VentaDTO>> GetLastSalesAsync()
    {
        var ventas = await this.GetAllVentasAsync();
        return ventas.AsParallel().OrderByDescending(v => v.Fecha).Take(5).ToList();
    }
}