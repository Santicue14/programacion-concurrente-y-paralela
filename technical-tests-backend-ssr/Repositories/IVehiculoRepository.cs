using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
public interface IVehiculoRepository
{
    Task<IEnumerable<Vehiculo>> GetAllAsync();
    Task<Vehiculo?> GetByIdAsync(int id);
    Task AddAsync(Vehiculo vehiculo);
    Task UpdateAsync(Vehiculo vehiculo);
    Task DeleteAsync(int id);
}
