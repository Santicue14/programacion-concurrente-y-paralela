using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface ITipoServicioRepository
{
    Task<IEnumerable<TipoServicio>> GetAllAsync();
    Task<TipoServicio?> GetByIdAsync(int id);
    Task<TipoServicio> AddAsync(TipoServicio tipoServicio);
    Task<TipoServicio> UpdateAsync(TipoServicio tipoServicio);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 