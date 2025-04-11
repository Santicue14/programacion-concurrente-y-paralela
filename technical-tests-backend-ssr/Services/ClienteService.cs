using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

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
    public Task<IEnumerable<Cliente>> GetAllClientsAsync()
    {
        return _clienteRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a client by ID asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Cliente?> GetClientByIdAsync(int id)
    {
        return _clienteRepository.GetByIdAsync(id);
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
        if (existingClient == null) return false;

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
        return await _clienteRepository.ExistsByTelefonoAsync(telefono);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _clienteRepository.ExistsByEmailAsync(email);
    }
}
