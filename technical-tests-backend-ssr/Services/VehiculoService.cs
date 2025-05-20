using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Service class for managing vehicles.
/// </summary>
public class VehiculoService
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly ICatalogoService _catalogoService;

    /// <summary>
    /// Constructor for the VehiculoService class.
    /// </summary>
    /// <param name="vehiculoRepository"></param>
    public VehiculoService(
        IVehiculoRepository vehiculoRepository, 
        ICatalogoService catalogoService)
    {
        _vehiculoRepository = vehiculoRepository;
        _catalogoService = catalogoService;
    }


    /// <summary>
    /// Get all vehicles.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<VehiculoDTO>> GetAllAsync()
    {
        // Obtener datos en paralelo
        var vehiculosTask = await _vehiculoRepository.GetAllAsync();
        var marcasTask = await _catalogoService.GetMarcasAsync();
        var modelosTask = await _catalogoService.GetModelosAsync();

        var vehiculos = vehiculosTask;
        var marcas = marcasTask;
        var modelos = modelosTask;

        // Crear un diccionario de marcas para acceso rápido
        var marcasDict = marcas
            .AsParallel()
            .ToDictionary(m => m.Id, m => m.Nombre);

        // Crear un diccionario de modelos con sus marcas
        var modelosDict = modelos.AsParallel()
            .ToDictionary(
                m => m.Id,
                m => new { m.Nombre, MarcaNombre = marcasDict.GetValueOrDefault(m.MarcaId, "Sin marca") }
            );

        // Mapear vehículos usando PLINQ
        return vehiculos.AsParallel().Select(v => new VehiculoDTO
        {
            Id = v.Id,
            Modelo = modelosDict.GetValueOrDefault(v.ModeloId)?.Nombre ?? "Sin modelo",
            Marca = modelosDict.GetValueOrDefault(v.ModeloId)?.MarcaNombre ?? "Sin marca",
            Anio = v.Anio,
            Precio = v.Precio,
            Stock = v.Stock
        });
    }

    /// <summary>
    /// Get a vehicle by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Vehiculo?> GetVehicleByIdAsync(int id)
    {
        return _vehiculoRepository.GetByIdAsync(id);
    }


    /// <summary>
    /// Add a new vehicle.
    /// </summary>
    /// <param name="vehiculo"></param>
    /// <returns></returns>
    public async Task<Vehiculo> AddVehicleAsync(Vehiculo vehiculo)
    {
        await _vehiculoRepository.AddAsync(vehiculo);
        return vehiculo;
    }

    /// <summary>
    /// Update an existing vehicle.
    /// </summary>
    /// <param name="vehiculo"></param>
    /// <returns></returns>
    public async Task<Vehiculo> UpdateVehicleAsync(Vehiculo vehiculo)
    {
        await _vehiculoRepository.UpdateAsync(vehiculo);
        return vehiculo;
    }

    /// <summary>
    /// Delete a vehicle by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteVehicleAsync(int id)
    {
        var existingVehicle = await _vehiculoRepository.GetByIdAsync(id);
        if (existingVehicle == null) return false;

        await _vehiculoRepository.DeleteAsync(id);
        return true;
    }
}
