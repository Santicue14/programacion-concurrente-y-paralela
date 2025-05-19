using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicioPosventaController : ControllerBase
{
    private readonly ServicioPosventaService _servicioService;
    private readonly ILogger<ServicioPosventaController> _logger;

    public ServicioPosventaController(
        ServicioPosventaService servicioService,
        ILogger<ServicioPosventaController> logger)
    {
        _servicioService = servicioService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioPosventaDTO>>> GetAllServicios()
    {
        try
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(servicios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener servicios de posventa");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServicioPosventaDTO>> GetServicioById(int id)
    {
        try
        {
            var servicio = await _servicioService.GetServicioByIdAsync(id);
            if (servicio == null)
                return NotFound();

            return Ok(servicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener servicio de posventa {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ServicioPosventaDTO>> CreateServicio(ServicioPosventaDTO servicioDTO)
    {
        try
        {
            var servicio = await _servicioService.CreateServicioAsync(servicioDTO);
            return CreatedAtAction(nameof(GetServicioById), new { id = servicio.Id }, servicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear servicio de posventa");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServicioPosventaDTO>> UpdateServicio(int id, ServicioPosventaDTO servicioDTO)
    {
        try
        {
            var servicio = await _servicioService.UpdateServicioAsync(id, servicioDTO);
            return Ok(servicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar servicio de posventa {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteServicio(int id)
    {
        try
        {
            var result = await _servicioService.DeleteServicioAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar servicio de posventa {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }


    [HttpPatch("{id}/estado")]
    public async Task<ActionResult<ServicioPosventaDTO>> UpdateEstado(int id, [FromBody] int nuevoEstado)
    {
        try
        {
            var servicio = await _servicioService.UpdateEstadoAsync(id, nuevoEstado);
            return Ok(servicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar estado del servicio de posventa {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 