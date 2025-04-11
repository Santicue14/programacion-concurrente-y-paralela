using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
/// <summary>
/// Interface for cliente repository
/// </summary>
public interface IClienteRepository
{
    /// <summary>
    /// Get all clientes from the database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Cliente>> GetAllAsync();
    /// <summary>
    /// Get a cliente by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Cliente?> GetByIdAsync(int id);
    /// <summary>
    /// Get a cliente by Id
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task AddAsync(Cliente cliente);
    /// <summary>
    /// Update a cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task UpdateAsync(Cliente cliente);
    /// <summary>
    /// Delete a cliente by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);
    /// <summary>
    /// Check if a cliente exists by id
    /// </summary>
    /// <param name="telefono"></param>
    /// <returns></returns>
    Task<bool> ExistsByTelefonoAsync(string telefono);
    /// <summary>
    /// Check if a cliente exists by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> ExistsByEmailAsync(string email);
}
