using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;

public class MarcaRepository : IMarcaRepository
{
    private readonly AppDbContext _context;



    public MarcaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Marca>> GetAllAsync()
    {
        return await _context.Marcas.ToListAsync();
    }

    public async Task<Marca?> GetByIdAsync(int id)
    {
        return await _context.Marcas.FindAsync(id);
    }

    public async Task<Marca> AddAsync(Marca marca)
    {
        await _context.Marcas.AddAsync(marca);
        await _context.SaveChangesAsync();
        return marca;
    }

    public async Task<Marca> UpdateAsync(Marca marca)
    {
        _context.Marcas.Update(marca);
        await _context.SaveChangesAsync();
        return marca;
    }

    public async Task DeleteAsync(int id)
    {
        var marca = await GetByIdAsync(id);
        if (marca != null)
        {
            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Marcas.AnyAsync(m => m.Id == id);
    }
} 