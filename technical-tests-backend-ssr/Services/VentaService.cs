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

    /// <summary>
    /// Constructor for VentaService.
    /// </summary>
    /// <param name="ventaRepository"></param>
    /// <param name="clienteRepository"></param>
    /// <param name="vehiculoRepository"></param>
    /// <param name="notificationService"></param>
    public VentaService(
        IVentaRepository ventaRepository, 
        IClienteRepository clienteRepository, 
        IVehiculoRepository vehiculoRepository,
        INotificationService notificationService)
    {
        _ventaRepository = ventaRepository;
        _clienteRepository = clienteRepository;
        _vehiculoRepository = vehiculoRepository;
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
        var vehiculos = await _vehiculoRepository.GetAllAsync();
        var clientes = await _clienteRepository.GetAllAsync();

        //Mapear vehiculos
        var vehiculosMapeados = vehiculos.AsParallel().Select(vehiculo => new VehiculoDTO
        {
            Id = vehiculo.Id,
            Marca = vehiculo.Modelo?.Marca?.Nombre ?? "Sin marca",
            Modelo = vehiculo.Modelo?.Nombre ?? "Sin modelo",
            Anio = vehiculo.Anio,
            Precio = vehiculo.Precio
        }).ToList();

        //Mapear clientes
        var clientesMapeados = clientes.AsParallel().Select(cliente => new ClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Email = cliente.Email,
            Telefono = cliente.Telefono
        }).ToList();

        var ventasMapeadas = ventas.AsParallel().Select(venta => new VentaDTO
        {
            Id = venta.Id,
            Cliente = clientesMapeados.FirstOrDefault(c => c.Id == venta.ClienteId),
            Vehiculo = vehiculosMapeados.FirstOrDefault(v => v.Id == venta.VehiculoId),
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
}
