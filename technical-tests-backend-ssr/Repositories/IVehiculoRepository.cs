using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
/// <summary>
/// Interface for the vehiculo repository
/// </summary>
public interface IVehiculoRepository
{
    /// <summary>
    /// Get all vehiculos from the database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Vehiculo>> GetAllAsync();

    /// <summary>
    /// Get vehiculo by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Vehiculo?> GetByIdAsync(int id);

    /// <summary>
    /// Add a new vehiculo to the database
    /// </summary>
    /// <param name="vehiculo"></param>
    /// <returns></returns>
    Task AddAsync(Vehiculo vehiculo);
    /// <summary>
    /// Update a vehiculo in the database
    /// </summary>
    /// <param name="vehiculo"></param>
    /// <returns></returns>
    Task UpdateAsync(Vehiculo vehiculo);
    /// <summary>
    /// Delete a vehiculo from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);
}
