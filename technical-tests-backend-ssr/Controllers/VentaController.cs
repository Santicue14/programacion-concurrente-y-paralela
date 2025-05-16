using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VentaController : ControllerBase
{
    private readonly VentaService _ventaService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ventaService"></param>
    /// <param name="mapper"></param>
    public VentaController(VentaService ventaService, IMapper mapper)
    {
        _ventaService = ventaService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los clientes.
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
    /// <param name="ventaDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<VentaDTO>> Create(VentaDTO ventaDTO)
    {
        // FluentValidation se hace autom�ticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var venta = _mapper.Map<Venta>(ventaDTO);

        Cliente cliente = await _clienteService.GetClienteByIdAsync(venta.ClienteId);
        Vehiculo vehiculo = await _vehiculoService.GetVehiculoByIdAsync(venta.VehiculoId);

        if (cliente == null)
        {
            return BadRequest("El cliente no existe.");
        }

        if (vehiculo == null)
        {
            return BadRequest("El vehiculo no existe.");
        }

        if (vehiculo.Stock <= 0)
        {
            return BadRequest("El vehiculo no está disponible.");
        }

        vehiculo.Stock--;

        await _vehiculoService.UpdateVehiculoAsync(vehiculo);

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
            return NotFound($"No se encontr� la venta con ID {id}.");
        }

        _mapper.Map(ventaDTO, venta);
        await _ventaService.UpdateVentaAsync(venta);

        var updatedClienteDTO = _mapper.Map<ClienteDTO>(cliente);
        return Ok(updatedClienteDTO); // Retornar el cliente actualizado
    }

    /// <summary>
    /// Eliminar un cliente.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _clienteService.DeleteClientAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
