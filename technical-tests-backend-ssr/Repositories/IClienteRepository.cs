using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task AddAsync(Cliente cliente);
    Task UpdateAsync(Cliente cliente);
    Task DeleteAsync(int id);
    Task<bool> ExistsByTelefonoAsync(string telefono);
    Task<bool> ExistsByEmailAsync(string email);
}
