using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly AppDbContext _context;

    public VehiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehiculo>> GetAllAsync()
    {
        return await _context.Vehiculos.ToListAsync();
    }

    public async Task<Vehiculo?> GetByIdAsync(int id)
    {
        return await _context.Vehiculos.FindAsync(id);
    }

    public async Task AddAsync(Vehiculo vehículo)
    {
        await _context.Vehiculos.AddAsync(vehículo);
        await _context.SaveChangesAsync();
    }

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