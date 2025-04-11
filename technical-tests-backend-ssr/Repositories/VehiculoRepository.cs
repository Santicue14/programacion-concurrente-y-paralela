using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

/// <summary>
/// Repositorio para gestionar las operaciones relacionadas con los vehículos.
/// </summary>
public class VehiculoRepository : IVehiculoRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor que inicializa el contexto de la base de datos.
    /// </summary>
    /// <param name="context"></param>
    public VehiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los vehículos de la base de datos.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Vehiculo>> GetAllAsync()
    {
        return await _context.Vehiculos
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un vehículo por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Vehiculo?> GetByIdAsync(int id)
    {
        return await _context.Vehiculos.FindAsync(id);
    }


    /// <summary>
    /// Agrega un nuevo vehículo a la base de datos.
    /// </summary>
    /// <param name="vehículo"></param>
    /// <returns></returns>
    public async Task AddAsync(Vehiculo vehículo)
    {
        await _context.Vehiculos.AddAsync(vehículo);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Actualiza un vehículo existente en la base de datos.
    /// </summary>
    /// <param name="vehículo"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task UpdateAsync(Vehiculo vehículo)
    {
        var existingVehiculo = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == vehículo.Id);

        if (existingVehiculo == null)
        {
            throw new KeyNotFoundException("El vehículo no existe.");
        }

        // Attach the updated entity and set its state to Modified
        _context.Attach(vehículo);
        _context.Entry(vehículo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Elimina un vehículo de la base de datos por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        var vehículo = await _context.Vehiculos.FindAsync(id);
        if (vehículo != null)
        {
            _context.Vehiculos.Remove(vehículo);
            await _context.SaveChangesAsync();
        }
    }

}