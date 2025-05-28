using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.Controllers;

/// <summary>
/// Controlador para gestionar analytics y estadísticas del dashboard
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    /// <summary>
    /// Constructor del controlador de analytics
    /// </summary>
    /// <param name="analyticsService">Servicio de analytics</param>
    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Obtiene las estadísticas principales del dashboard
    /// </summary>
    /// <param name="startDate">Fecha de inicio para filtrar (opcional)</param>
    /// <param name="endDate">Fecha de fin para filtrar (opcional)</param>
    /// <param name="vehicleBrand">Marca de vehículo para filtrar (opcional)</param>
    /// <param name="clientId">ID del cliente para filtrar (opcional)</param>
    /// <returns>Estadísticas del dashboard</returns>
    [HttpGet("dashboard-stats")]
    public async Task<ActionResult<DashboardStatsDTO>> GetDashboardStats(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? vehicleBrand = null,
        [FromQuery] int? clientId = null)
    {
        try
        {
            var stats = await _analyticsService.GetDashboardStatsAsync(startDate, endDate, vehicleBrand, clientId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene el número total de vehículos en catálogo
    /// </summary>
    /// <returns>Cantidad de vehículos disponibles</returns>
    [HttpGet("total-vehicles")]
    public async Task<ActionResult<int>> GetTotalVehicles()
    {
        try
        {
            var totalVehicles = await _analyticsService.GetTotalVehiclesAsync();
            return Ok(totalVehicles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
} 