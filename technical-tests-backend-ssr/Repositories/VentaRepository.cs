using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;
/// <summary>
/// Interface for venta repository
/// </summary>
public class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor for VentaRepository
    /// </summary>
    /// <param name="context"></param>
    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all ventas from the database
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Venta>> GetAllAsync()
    {
        return await _context.Ventas.ToListAsync();
    }
    /// <summary>
    /// Get a venta by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _context.Ventas.FindAsync(id);
    }
    /// <summary>
    /// Get a venta by Id
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    public async Task AddAsync(Venta venta)
    {
        await _context.Ventas.AddAsync(venta);
        await _context.SaveChangesAsync();
    }
    /// <summary>
    /// Update a venta
    /// </summary>
    /// <param name="venta"></param>
    /// <returns></returns>
    public async Task UpdateAsync(Venta venta)
    {
        _context.Ventas.Update(venta);
        await _context.SaveChangesAsync();
    }
    /// <summary>
    /// Delete a venta by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        var venta = await GetByIdAsync(id);
        if (venta != null)
        {
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
    }

}
