using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public class ModeloRepository : IModeloRepository
{
    private readonly AppDbContext _context;

    public ModeloRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Modelo>> GetAllAsync()
    {
        return await _context.Modelos.Include(m => m.Marca).ToListAsync();
    }

    public async Task<Modelo?> GetByIdAsync(int id)
    {
        return await _context.Modelos.Include(m => m.Marca).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Modelo> AddAsync(Modelo modelo)
    {
        await _context.Modelos.AddAsync(modelo);
        await _context.SaveChangesAsync();
        return modelo;
    }

    public async Task<Modelo> UpdateAsync(Modelo modelo)
    {
        _context.Modelos.Update(modelo);
        await _context.SaveChangesAsync();
        return modelo;
    }

    public async Task DeleteAsync(int id)
    {
        var modelo = await GetByIdAsync(id);
        if (modelo != null)
        {
            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Modelos.AnyAsync(m => m.Id == id);
    }
} 