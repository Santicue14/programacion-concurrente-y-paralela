using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

public class VehiculoService
{
    private readonly IVehiculoRepository _vehiculoRepository;

    public VehiculoService(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public Task<IEnumerable<Vehiculo>> GetAllVehiclesAsync()
    {
        return _vehiculoRepository.GetAllAsync();
    }

    public Task<Vehiculo?> GetVehicleByIdAsync(int id)
    {
        return _vehiculoRepository.GetByIdAsync(id);
    }

    public async Task<Vehiculo> AddVehicleAsync(Vehiculo vehiculo)
    {
        await _vehiculoRepository.AddAsync(vehiculo);
        return vehiculo;
    }

    public async Task<Vehiculo> UpdateClientAsync(Vehiculo vehiculo)
    {
        await _vehiculoRepository.UpdateAsync(vehiculo);
        return vehiculo;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var existingVehicle = await _vehiculoRepository.GetByIdAsync(id);
        if (existingVehicle == null) return false;

        await _vehiculoRepository.DeleteAsync(id);
        return true;
    }
}
