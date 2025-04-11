using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public Task<IEnumerable<Cliente>> GetAllClientsAsync()
    {
        return _clienteRepository.GetAllAsync();
    }

    public Task<Cliente?> GetClientByIdAsync(int id)
    {
        return _clienteRepository.GetByIdAsync(id);
    }

    public async Task<Cliente> AddClientAsync(Cliente cliente)
    {
        await _clienteRepository.AddAsync(cliente);
        return cliente;
    }

    public async Task<Cliente> UpdateClientAsync(Cliente cliente)
    {
        await _clienteRepository.UpdateAsync(cliente);
        return cliente;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var existingClient = await _clienteRepository.GetByIdAsync(id);
        if (existingClient == null) return false;

        await _clienteRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> ExistsByTelefonoAsync(string telefono)
    {
        return await _clienteRepository.ExistsByTelefonoAsync(telefono);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _clienteRepository.ExistsByEmailAsync(email);
    }
}
