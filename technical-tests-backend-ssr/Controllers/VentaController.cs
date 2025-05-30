using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Services;

/// <summary>
/// Controlador para gestionar las ventas
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VentaController : ControllerBase
{
    private readonly VentaService _ventaService;
    private readonly ClienteService _clienteService;
    private readonly VehiculoService _vehiculoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor del controlador de ventas
    /// </summary>
    /// <param name="ventaService"></param>
    /// <param name="clienteService"></param>
    /// <param name="vehiculoService"></param>
    /// <param name="mapper"></param>
    public VentaController(
        VentaService ventaService, 
        ClienteService clienteService,
        VehiculoService vehiculoService,
        IMapper mapper)
    {
        _ventaService = ventaService;
        _clienteService = clienteService;
        _vehiculoService = vehiculoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todas las ventas.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VentaDTO>>> GetAll()
    {
        var ventas = await _ventaService.GetAllVentasAsync();
        return Ok(_mapper.Map<IEnumerable<VentaDTO>>(ventas));
    }

    /// <summary>
    /// Obtener una venta por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<VentaDTO>> GetById(int id)
    {
        var venta = await _ventaService.GetVentaByIdAsync(id);
        if (venta == null) return NotFound();
        return Ok(_mapper.Map<VentaDTO>(venta));
    }


    /// <summary>
    /// Agregar una nueva venta    
    /// </summary>
    /// <param name="request">Objeto con clientId y vehicleId</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<VentaDTO>> Create([FromBody] CreateVentaRequest request)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cliente = await _clienteService.GetClientByIdAsync(request.ClientId);
        if (cliente == null)
        {
            return NotFound("El cliente no existe.");
        }
        var vehiculo = await _vehiculoService.GetVehicleByIdAsync(request.VehicleId);
        if (vehiculo == null)
        {
            return NotFound("El vehiculo no existe.");
        }
        var venta = new Venta
        {
            ClienteId = request.ClientId,
            VehiculoId = request.VehicleId,
            Total = vehiculo.Precio
        };

        await _vehiculoService.UpdateVehicleAsync(vehiculo);

        var newVenta = await _ventaService.AddVentaAsync(venta);
        return CreatedAtAction(nameof(GetById), new { id = newVenta.Id }, _mapper.Map<VentaDTO>(newVenta));
    }


    /// <summary>
    /// Modificar una venta existente
    /// </summary>
    /// <param name="ventaDTO"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<VentaDTO>> Update(int id, [FromBody] VentaDTO ventaDTO)
    {
       if (id != ventaDTO.Id)
       {
           return BadRequest("El ID de la venta no coincide con el de la URL.");
       }

        var venta = await _ventaService.GetVentaByIdAsync(id);
        if (venta == null)
        {
            return NotFound($"No se encontró la venta con ID {id}.");
        }

        _mapper.Map(ventaDTO, venta);
        await _ventaService.UpdateVentaAsync(venta);

        var updatedVentaDTO = _mapper.Map<VentaDTO>(venta);
        return Ok(updatedVentaDTO);
    }

    /// <summary>
    /// Eliminar una venta.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _ventaService.DeleteVentaAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Obtiene el número total de ventas
    /// </summary>
    /// <returns>Total de ventas</returns>
    [HttpGet("total-sales")]
    public async Task<int> GetTotalSalesAsync()
    {
        return await _ventaService.GetTotalSalesAsync();
    }

    /// <summary>
    /// Obtiene el total de ingresos
    /// </summary>
    /// <returns>Total de ingresos</returns>
    [HttpGet("total-revenue")]
    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _ventaService.GetTotalRevenueAsync();
    }

    /// <summary>
    /// Obtiene el total de ventas del mes actual
    /// </summary>
    /// <returns>Total de ventas del mes actual</returns>
    [HttpGet("sales-this-month")]
    public async Task<int> GetSalesThisMonthAsync()
    {
        return await _ventaService.GetSalesThisMonthAsync();
    }

    /// <summary>
    /// Obtiene el total de ingresos del mes actual
    /// </summary>
    /// <returns>Total de ingresos del mes actual</returns>
    [HttpGet("revenue-this-month")]
    public async Task<decimal> GetRevenueThisMonthAsync()
    {
        return await _ventaService.GetRevenueThisMonthAsync();
    }
    
    /// <summary>
    /// Obtiene las 5 marcas más vendidas con su respectivo total de ventas y porcentaje de ventas
    /// </summary>
    /// <returns></returns>
    [HttpGet("sales-by-brand")]
    public async Task<IEnumerable<Object>> GetSalesByBrandAsync()
    {
        return await _ventaService.GetSalesByBrandAsync();
    }

    /// <summary>
    /// Obtiene las 10 modelos más vendidos con su respectivo total de ventas y porcentaje de ventas
    /// </summary>
    /// <returns>VentaDTO</returns>
    [HttpGet("sales-by-model")]
    public async Task<IEnumerable<Object>> GetSalesByModelAsync()
    {
        return await _ventaService.GetSalesByModelAsync();
    }
    /// <summary>
    /// Obtener últimas 5 ventas
    /// </summary>
    /// <returns>Últimas 5 ventas</returns>
    [HttpGet("last-sales")]
    public async Task<IEnumerable<VentaDTO>> GetLastSalesAsync()
    {
        return await _ventaService.GetLastSalesAsync();
    }
}

