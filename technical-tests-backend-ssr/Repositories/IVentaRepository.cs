using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
/// <summary>
/// Interface for venta repository
/// </summary>
public interface IVentaRepository
{
    /// <summary>
    /// Get all ventas from the database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Venta>> GetAllAsync();
    /// <summary>
    /// Get a venta by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Venta?> GetByIdAsync(int id);
    /// <summary>
    /// Get a venta by Id
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    Task AddAsync(Venta venta);
    /// <summary>
    /// Update a venta
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    Task UpdateAsync(Venta venta);
    /// <summary>
    /// Delete a venta by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);

}
