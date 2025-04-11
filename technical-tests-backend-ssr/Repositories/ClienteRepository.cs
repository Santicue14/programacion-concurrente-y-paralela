using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;
/// <summary>
/// Implementación de la interfaz IClienteRepository.
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;
    /// <summary>
    /// Constructor de la clase ClienteRepository.
    /// </summary>
    /// <param name="context"></param>
    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los clientes de la base de datos.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    /// <summary>
    /// Agrega un nuevo cliente a la base de datos.
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Actualiza un cliente existente en la base de datos.
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task UpdateAsync(Cliente cliente)
    {
        var existingCliente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == cliente.Id);

        if (existingCliente == null)
        {
            throw new KeyNotFoundException("El cliente no existe.");
        }

        // Attach the updated entity and set its state to Modified
        _context.Attach(cliente);
        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Elimina un cliente de la base de datos por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Verifica si un cliente existe en la base de datos por su ID.
    /// </summary>
    /// <param name="telefono"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByTelefonoAsync(string telefono)
    {
        return await _context.Clientes.AnyAsync(c => c.Telefono == telefono);
    }

    /// <summary>
    /// Verifica si un cliente existe en la base de datos por su email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Clientes.AnyAsync(c => c.Email == email);
    }


}