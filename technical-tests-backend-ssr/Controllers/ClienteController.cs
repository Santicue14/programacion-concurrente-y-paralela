using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _clienteService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientService"></param>
    /// <param name="mapper"></param>
    public ClienteController(ClienteService clientService, IMapper mapper)
    {
        _clienteService = clientService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los clientes.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
    {
        var clientes = await _clienteService.GetAllClientsAsync();
        return Ok(_mapper.Map<IEnumerable<ClienteDTO>>(clientes));
    }

    /// <summary>
    /// Obtener un cliente por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> GetById(int id)
    {
        var cliente = await _clienteService.GetClientByIdAsync(id);
        if (cliente == null) return NotFound();
        return Ok(_mapper.Map<ClienteDTO>(cliente));
    }


    /// <summary>
    /// Agregar un nuevo cliente    
    /// </summary>
    /// <param name="clienteDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Create(ClienteDTO clienteDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cliente = _mapper.Map<Cliente>(clienteDTO);

        //Valida si el teléfono ya existe
        if (await _clienteService.ExistsByTelefonoAsync(cliente.Telefono))
        {
            return BadRequest("El teléfono ya está en uso.");
        }

        //Valida si el email ya existe
        if (await _clienteService.ExistsByEmailAsync(cliente.Email))
        {
            return BadRequest("El email ya está en uso.");
        }

        var newCliente = await _clienteService.AddClientAsync(cliente);
        return CreatedAtAction(nameof(GetById), new { id = newCliente.Id }, _mapper.Map<ClienteDTO>(newCliente));
    }


    /// <summary>
    /// Modificar un cliente existente
    /// </summary>
    /// <param name="clienteDTO"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Update(int id, [FromBody] ClienteDTO clienteDTO)
    {
       if (id != clienteDTO.Id)
       {
           return BadRequest("El ID del cliente no coincide con el de la URL.");
       }

        var cliente = await _clienteService.GetClientByIdAsync(id);
        if (cliente == null)
        {
            return NotFound($"No se encontró el cliente con ID {id}.");
        }

        _mapper.Map(clienteDTO, cliente);
        await _clienteService.UpdateClientAsync(cliente);

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
