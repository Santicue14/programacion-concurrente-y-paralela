using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.DTOs;

namespace technical_tests_backend_ssr.Services;

public interface ICatalogoService
{
    /// <summary>
    /// Obtiene todos los tipos de servicio.
    /// </summary>
    /// <returns>Una lista de tipos de servicio.</returns>
    Task<IEnumerable<TipoServicioDTO>> GetTiposServicioAsync();

    /// <summary>
    /// Agrega un nuevo tipo de servicio.
    /// </summary>
    /// <param name="tipoServicioDTO">El tipo de servicio a agregar.</param>
    /// <returns>El tipo de servicio agregado.</returns>
    Task<TipoServicioDTO> AddTipoServicioAsync(TipoServicioDTO tipoServicioDTO);

    /// <summary>
    /// Actualiza un tipo de servicio.
    /// </summary>
    /// <param name="id">El identificador único del tipo de servicio.</param>
    /// <param name="tipoServicioDTO">El tipo de servicio a actualizar.</param>
    /// <returns>El tipo de servicio actualizado.</returns>
    Task<TipoServicioDTO> UpdateTipoServicioAsync(int id, TipoServicioDTO tipoServicioDTO);

    /// <summary>
    /// Elimina un tipo de servicio.
    /// </summary>
    Task DeleteTipoServicioAsync(int id);

    /// <summary>
    /// Obtiene todos los modelos de vehículos.
    /// </summary>
    /// <returns>Una lista de modelos de vehículos.</returns>
    Task<IEnumerable<ModeloDTO>> GetModelosAsync();

    /// <summary>
    /// Agrega un nuevo modelo de vehículo.
    /// </summary>
    /// <param name="modeloDTO">El modelo de vehículo a agregar.</param>
    /// <returns>El modelo de vehículo agregado.</returns>
    Task<ModeloDTO> AddModeloAsync(ModeloDTO modeloDTO);

    /// <summary>
    /// Obtiene todas las marcas de vehículos.
    /// </summary>
    /// <returns>Una lista de marcas de vehículos.</returns>
    Task<IEnumerable<MarcaDTO>> GetMarcasAsync();

    /// <summary>
    /// Agrega una nueva marca de vehículo.
    /// </summary>
    /// <param name="marcaDTO">La marca de vehículo a agregar.</param>
    /// <returns>La marca de vehículo agregada.</returns>
    Task<MarcaDTO> AddMarcaAsync(MarcaDTO marcaDTO);
} 