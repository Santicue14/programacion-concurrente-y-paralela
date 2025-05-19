using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public class TipoServicioRepository : ITipoServicioRepository
{
    private readonly AppDbContext _context;

    public TipoServicioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoServicio>> GetAllAsync()
    {
        return await _context.TiposServicio.ToListAsync();
    }

    public async Task<TipoServicio?> GetByIdAsync(int id)
    {
        return await _context.TiposServicio.FindAsync(id);
    }

    public async Task<TipoServicio> AddAsync(TipoServicio tipoServicio)
    {
        await _context.TiposServicio.AddAsync(tipoServicio);
        await _context.SaveChangesAsync();
        return tipoServicio;
    }

    public async Task<TipoServicio> UpdateAsync(TipoServicio tipoServicio)
    {
        _context.TiposServicio.Update(tipoServicio);
        await _context.SaveChangesAsync();
        return tipoServicio;
    }

    public async Task DeleteAsync(int id)
    {
        var tipoServicio = await GetByIdAsync(id);
        if (tipoServicio != null)
        {
            _context.TiposServicio.Remove(tipoServicio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.TiposServicio.AnyAsync(t => t.Id == id);
    }
} 