using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogoController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;
    private readonly ILogger<CatalogoController> _logger;

    public CatalogoController(ICatalogoService catalogoService, ILogger<CatalogoController> logger)
    {
        _catalogoService = catalogoService;
        _logger = logger;
    }

    #region Tipos de Servicio
    [HttpGet("tipos-servicio")]
    public async Task<ActionResult<IEnumerable<TipoServicioDTO>>> GetTiposServicio()
    {
        try
        {
            var tipos = await _catalogoService.GetTiposServicioAsync();
            return Ok(tipos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de servicio");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("tipos-servicio")]
    public async Task<ActionResult<TipoServicioDTO>> AddTipoServicio([FromBody] TipoServicioDTO tipoServicioDTO)
    {
        try
        {
            var tipo = await _catalogoService.AddTipoServicioAsync(tipoServicioDTO);
            return CreatedAtAction(nameof(GetTiposServicio), tipo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar tipo de servicio");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("tipos-servicio/{id}")]
    public async Task<ActionResult<TipoServicioDTO>> UpdateTipoServicio(int id, [FromBody] TipoServicioDTO tipoServicioDTO)
    {
        try
        {
            var tipo = await _catalogoService.UpdateTipoServicioAsync(id, tipoServicioDTO);
            return Ok(tipo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar tipo de servicio {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("tipos-servicio/{id}")]
    public async Task<ActionResult> DeleteTipoServicio(int id)
    {
        try
        {
            await _catalogoService.DeleteTipoServicioAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar tipo de servicio {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    #endregion

    #region Modelos
    [HttpGet("modelos")]
    public async Task<ActionResult<IEnumerable<ModeloDTO>>> GetModelos()
    {
        try
        {
            var modelos = await _catalogoService.GetModelosAsync();
            return Ok(modelos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener modelos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("modelos")]
    public async Task<ActionResult<ModeloDTO>> AddModelo([FromBody] ModeloDTO modeloDTO)
    {
        try
        {
            var modelo = await _catalogoService.AddModeloAsync(modeloDTO);
            return CreatedAtAction(nameof(GetModelos), modelo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar modelo");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    #endregion

    #region Marcas
    [HttpGet("marcas")]
    public async Task<ActionResult<IEnumerable<MarcaDTO>>> GetMarcas()
    {
        try
        {
            var marcas = await _catalogoService.GetMarcasAsync();
            return Ok(marcas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener marcas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("marcas")]
    public async Task<ActionResult<MarcaDTO>> AddMarca([FromBody] MarcaDTO marcaDTO)
    {
        try
        {
            var marca = await _catalogoService.AddMarcaAsync(marcaDTO);
            return CreatedAtAction(nameof(GetMarcas), marca);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar marca");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    #endregion
} 