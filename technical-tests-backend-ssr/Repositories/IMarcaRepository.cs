using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public interface IMarcaRepository
{
    /// <summary>       
    /// Obtiene todas las marcas de vehículos.
    /// </summary>
    /// <returns>Una lista de marcas de vehículos.</returns>
    Task<IEnumerable<Marca>> GetAllAsync();

    /// <summary>
    /// Obtiene una marca de vehículo por su identificador único.
    /// </summary>  
    /// <param name="id">El identificador único de la marca.</param>
    /// <returns>La marca de vehículo correspondiente al identificador proporcionado.</returns>
    Task<Marca?> GetByIdAsync(int id);

    /// <summary>
    /// Agrega una nueva marca de vehículo.
    /// </summary>
    /// <param name="marca">La marca de vehículo a agregar.</param>
    /// <returns>La marca de vehículo agregada.</returns>
    Task<Marca> AddAsync(Marca marca);

    /// <summary>
    /// Actualiza una marca de vehículo existente.
    /// </summary>
    /// <param name="marca">La marca de vehículo a actualizar.</param>
    /// <returns>La marca de vehículo actualizada.</returns>
    Task<Marca> UpdateAsync(Marca marca);

    /// <summary>
    /// Elimina una marca de vehículo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la marca.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Verifica si una marca de vehículo existe por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la marca.</param>
    /// <returns>True si la marca existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(int id);
} 