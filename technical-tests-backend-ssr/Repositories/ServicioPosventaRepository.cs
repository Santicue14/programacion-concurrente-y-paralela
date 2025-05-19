using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Repositories;

public class ServicioPosventaRepository : IServicioPosventaRepository
{
    private readonly AppDbContext _context;

    public ServicioPosventaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServicioPosventa>> GetAllAsync()
    {
        return await _context.ServiciosPosventa
            .ToListAsync();
    }

    public async Task<ServicioPosventa?> GetByIdAsync(int id)
    {
        return await _context.ServiciosPosventa.FindAsync(id);
    }

    public async Task<ServicioPosventa> AddAsync(ServicioPosventa servicio)
    {
        servicio.FechaSolicitud = DateTime.UtcNow;
        servicio.Estado = (int)EstadoServicio.Pendiente;
        
        _context.ServiciosPosventa.Add(servicio);
        await _context.SaveChangesAsync();
        return servicio;
    }

    public async Task<ServicioPosventa> UpdateAsync(ServicioPosventa servicio)
    {
        _context.Entry(servicio).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return servicio;
    }

    public async Task DeleteAsync(int id)
    {
        var servicio = await _context.ServiciosPosventa.FindAsync(id);
        if (servicio != null)
        {
            _context.Remove(servicio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ServicioPosventa> UpdateEstadoAsync(int id, int nuevoEstado)
    {
        var servicio = await _context.ServiciosPosventa.FindAsync(id);
        if (servicio == null)
            throw new Exception("Servicio no encontrado");

        servicio.Estado = nuevoEstado;
        await _context.SaveChangesAsync();
        return servicio;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ServiciosPosventa.AnyAsync(e => e.Id == id);
    }
} 