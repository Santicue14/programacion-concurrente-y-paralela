using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

/// <summary>
/// Service class for managing vehicles.
/// </summary>
public class VehiculoService
{
    private readonly IVehiculoRepository _vehiculoRepository;


    /// <summary>
    /// Constructor for the VehiculoService class.
    /// </summary>
    /// <param name="vehiculoRepository"></param>
    public VehiculoService(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }


    /// <summary>
    /// Get all vehicles.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<VehiculoDTO>> GetAllAsync()
    {
        var vehiculos = await _vehiculoRepository.GetAllAsync(); 

        return vehiculos.Select(v => new VehiculoDTO
        {
            Id = v.Id,
            Marca = v.Modelo?.Marca?.Nombre ?? "Sin marca",
            Modelo = v.Modelo?.Nombre ?? "Sin modelo",
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
