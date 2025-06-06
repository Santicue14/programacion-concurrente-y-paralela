using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Service class for managing Cliente entities.
/// </summary>
public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;

    /// <summary>
    /// Constructor for ClienteService.
    /// </summary>
    /// <param name="clienteRepository"></param>
    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Retrieves all clients asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Cliente>> GetAllClientsAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        if (clientes == null) throw new UserNotFoundException();
        return clientes;
    }

    /// <summary>
    /// Retrieves a client by ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Cliente?> GetClientByIdAsync(int id)
    {
        var cliente = _clienteRepository.GetByIdAsync(id);
        if (cliente == null) throw new UserNotFoundException();
        return cliente;
    }


    /// <summary>
    /// Adds a new client asynchronously.
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    public async Task<Cliente> AddClientAsync(Cliente cliente)
    {
        await _clienteRepository.AddAsync(cliente);
        return cliente;
    }

    /// <summary>
    /// Updates an existing client asynchronously.
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    public async Task<Cliente> UpdateClientAsync(Cliente cliente)
    {
        var existingClient = await _clienteRepository.GetByIdAsync(cliente.Id);
        if (existingClient == null) throw new UserNotFoundException();
        await _clienteRepository.UpdateAsync(cliente);
        return cliente;
    }

    /// <summary>
    /// Deletes a client by ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteClientAsync(int id)
    {
        var existingClient = await _clienteRepository.GetByIdAsync(id);
        if (existingClient == null) throw new UserNotFoundException();

        await _clienteRepository.DeleteAsync(id);
        return true;
    }

    /// <summary>
    /// Checks if a client exists by ID asynchronously.
    /// </summary>
    /// <param name="telefono"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByTelefonoAsync(string telefono)
    {
        var exists = await _clienteRepository.ExistsByTelefonoAsync(telefono);
        if (!exists) throw new UserNotFoundException();
        return exists;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var exists = await _clienteRepository.ExistsByEmailAsync(email);
        if (!exists) throw new UserNotFoundException();
        return exists;
    }

    /// <summary>
    /// Obtiene el numero total de clientes
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTotalClientsAsync()
    {
        var clientes = await this.GetAllClientsAsync();
        return clientes.Count();
    }
}
