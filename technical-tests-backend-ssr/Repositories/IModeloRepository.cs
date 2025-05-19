using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface IModeloRepository
{
    Task<IEnumerable<Modelo>> GetAllAsync();
    Task<Modelo?> GetByIdAsync(int id);
    Task<Modelo> AddAsync(Modelo modelo);
    Task<Modelo> UpdateAsync(Modelo modelo);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 