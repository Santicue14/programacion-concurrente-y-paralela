using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Repositories;
using System.Diagnostics;
using AutoMapper;
namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Servicio para gestionar las solicitudes de servicio de posventa.
/// </summary>
public class ServicioPosventaService
{
    private readonly IServicioPosventaRepository _servicioRepository;
    private readonly IVentaRepository _ventaRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<ServicioPosventaService> _logger;
    private readonly IMapper _mapper;

    public ServicioPosventaService(
        IServicioPosventaRepository servicioRepository,
        IVentaRepository ventaRepository,
        INotificationService notificationService,
        ILogger<ServicioPosventaService> logger,
        IMapper mapper)
    {
        _servicioRepository = servicioRepository;
        _ventaRepository = ventaRepository;
        _notificationService = notificationService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las solicitudes de servicio de posventa.
    /// </summary>
    /// <returns>Una lista de solicitudes de servicio de posventa.</returns>
    public async Task<IEnumerable<ServicioPosventaDTO>> GetAllServiciosAsync()
    {
        var cronometro = Stopwatch.StartNew();
        var servicios = await _servicioRepository.GetAllAsync();
        
        cronometro.Stop();
        _logger.LogInformation($"Tiempo de ejecución de consulta de servicios: {cronometro.ElapsedMilliseconds} ms");
        
        return _mapper.Map<IEnumerable<ServicioPosventaDTO>>(servicios);
    }

    /// <summary>
    /// Obtiene una solicitud de servicio de posventa por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <returns>La solicitud de servicio de posventa correspondiente al identificador proporcionado.</returns>
    public async Task<ServicioPosventaDTO?> GetServicioByIdAsync(int id)
    {
        var servicio = await _servicioRepository.GetByIdAsync(id);
        if (servicio == null) return null;

        return _mapper.Map<ServicioPosventaDTO>(servicio);
    }

    /// <summary>
    /// Crea una nueva solicitud de servicio de posventa.
    /// </summary>
    /// <param name="servicioDTO">La solicitud de servicio de posventa a crear.</param>
    /// <returns>La solicitud de servicio de posventa creada.</returns>
    public async Task<ServicioPosventaDTO> CreateServicioAsync(ServicioPosventaDTO servicioDTO)
    {
        var servicio = _mapper.Map<ServicioPosventa>(servicioDTO);

        var servicioCreado = await _servicioRepository.AddAsync(servicio);

        // Enviar notificación según el tipo de servicio
        switch (servicioDTO.TipoServicio.ToUpper())
        {
            case "MANTENIMIENTO":
                if (servicioDTO.FechaProgramada.HasValue)
                    await _notificationService.SendMaintenanceServiceRequestAsync(servicioDTO.Descripcion, servicioDTO.FechaProgramada.Value);
                break;
            case "GARANTIA":
                await _notificationService.SendWarrantyClaimNotificationAsync(servicioDTO.Descripcion);
                break;
            case "RECLAMO":
                await _notificationService.SendCustomerComplaintNotificationAsync(servicioDTO.Descripcion);
                break;
        }

        return _mapper.Map<ServicioPosventaDTO>(servicioCreado);
    }

    /// <summary>
    /// Actualiza una solicitud de servicio de posventa.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <param name="servicioDTO">La solicitud de servicio de posventa a actualizar.</param>
    /// <returns>La solicitud de servicio de posventa actualizada.</returns>
    public async Task<ServicioPosventaDTO> UpdateServicioAsync(int id, ServicioPosventaDTO servicioDTO)
    {
        var servicio = await _servicioRepository.GetByIdAsync(id);
        if (servicio == null)
            throw new Exception("Servicio no encontrado");

        servicio = _mapper.Map<ServicioPosventa>(servicioDTO);

        var servicioActualizado = await _servicioRepository.UpdateAsync(servicio);

        return _mapper.Map<ServicioPosventaDTO>(servicioActualizado);
    }

    /// <summary>
    /// Elimina una solicitud de servicio de posventa.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <returns>True si la solicitud de servicio de posventa se eliminó correctamente, False en caso contrario.</returns>
    public async Task<bool> DeleteServicioAsync(int id)
    {
        var servicio = await _servicioRepository.GetByIdAsync(id);
        if (servicio == null) return false;

        await _servicioRepository.DeleteAsync(id);
        return true;
    }

    /// <summary>
    /// Actualiza el estado de una solicitud de servicio de posventa.
    /// </summary>
    /// <param name="id">El identificador único de la solicitud de servicio de posventa.</param>
    /// <param name="nuevoEstado">El nuevo estado de la solicitud de servicio de posventa.</param>
    /// <returns>La solicitud de servicio de posventa actualizada.</returns>
    public async Task<ServicioPosventaDTO> UpdateEstadoAsync(int id, int nuevoEstado)
    {
        var servicio = await _servicioRepository.UpdateEstadoAsync(id, nuevoEstado);
        
        if (servicio == null)
        {
            throw new Exception("Servicio no encontrado"); 
        }

        return _mapper.Map<ServicioPosventaDTO>(servicio);
    }
} 