using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

/// <summary>
/// Interfaz para el repositorio de solicitudes de servicio de posventa.
/// </summary>
public interface IServicioPosventaRepository
{
    /// <summary>   
    /// Obtiene todas las solicitudes de servicio de posventa.
    /// </summary>
    /// <returns>Una lista de solicitudes de servicio de posventa.</returns>
    Task<IEnumerable<ServicioPosventa>> GetAllAsync();

    /// <summary>
    /// Obtiene una solicitud de servicio de posventa por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <returns>La solicitud de servicio de posventa correspondiente al identificador proporcionado.</returns>
    Task<ServicioPosventa?> GetByIdAsync(int id);

    /// <summary>
    /// Agrega una nueva solicitud de servicio de posventa.
    /// </summary>
    /// <param name="servicioPosventa">La solicitud de servicio de posventa a agregar.</param>
    /// <returns>La solicitud de servicio de posventa agregada.</returns>
    Task<ServicioPosventa> AddAsync(ServicioPosventa servicioPosventa);

    /// <summary>
    /// Actualiza una solicitud de servicio de posventa existente.
    /// </summary>
    /// <param name="servicioPosventa">La solicitud de servicio de posventa a actualizar.</param>
    /// <returns>La solicitud de servicio de posventa actualizada.</returns>
    Task<ServicioPosventa> UpdateAsync(ServicioPosventa servicioPosventa);

    /// <summary>
    /// Elimina una solicitud de servicio de posventa por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Verifica si una solicitud de servicio de posventa existe por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <returns>True si la solicitud de servicio de posventa existe, False en caso contrario.</returns>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Actualiza el estado de una solicitud de servicio de posventa.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <param name="nuevoEstado">El nuevo estado de la solicitud de servicio de posventa.</param>
    Task<ServicioPosventa> UpdateEstadoAsync(int id, int nuevoEstado);
} 