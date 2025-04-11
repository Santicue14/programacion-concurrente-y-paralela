using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VehiculoController : ControllerBase
{
    private readonly VehiculoService _vehiculoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vehiculoService"></param>
    /// <param name="mapper"></param>
    public VehiculoController(VehiculoService vehiculoService, IMapper mapper)
    {
        _vehiculoService = vehiculoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los vehiculo.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehiculos = await _vehiculoService.GetAllAsync();
        return Ok(vehiculos);
    }

    /// <summary>
    /// Obtener un vehiculo por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<VehiculoDTO>> GetById(int id)
    {
        var vehiculo = await _vehiculoService.GetVehicleByIdAsync(id);
        if (vehiculo == null) return NotFound();
        return Ok(_mapper.Map<VehiculoDTO>(vehiculo));
    }


    /// <summary>
    /// Agregar un nuevo vehículo    
    /// </summary>
    /// <param name="vehiculoDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<VehiculoDTO>> Create(VehiculoDTO vehiculoDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vehiculo = _mapper.Map<Vehiculo>(vehiculoDTO);

        var newVehiculo = await _vehiculoService.AddVehicleAsync(vehiculo);
        return CreatedAtAction(nameof(GetById), new { id = newVehiculo.Id }, _mapper.Map<VehiculoDTO>(newVehiculo));
    }


    /// <summary>
    /// Modificar un vehículo existente
    /// </summary>
    /// <param name="vehiculoDTO"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<VehiculoDTO>> Update(int id, [FromBody] VehiculoDTO vehiculoDTO)
    {
       if (id != vehiculoDTO.Id)
       {
           return BadRequest("El ID del vehículo no coincide con el de la URL.");
       }

        var vehiculo = await _vehiculoService.GetVehicleByIdAsync(id);
        if (vehiculo == null)
        {
            return NotFound($"No se encontró el vehículo con ID {id}.");
        }

        _mapper.Map(vehiculoDTO, vehiculo);
        await _vehiculoService.UpdateVehicleAsync(vehiculo);

        var updatedVehicleTO = _mapper.Map<VehiculoDTO>(vehiculo);
        return Ok(updatedVehicleTO); // Retornar el vehículo actualizado
    }

    /// <summary>
    /// Eliminar un veh´culo.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _vehiculoService.DeleteVehicleAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
