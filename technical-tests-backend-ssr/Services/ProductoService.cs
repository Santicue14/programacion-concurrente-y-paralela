using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

public class ProductService
{
    private readonly IProductoRepository _productoRepository;

    public ProductService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public Task<IEnumerable<Producto>> GetAllProductsAsync()
    {
        return _productoRepository.GetAllAsync();
    }

    public Task<Producto?> GetProductByIdAsync(int id)
    {
        return _productoRepository.GetByIdAsync(id);
    }

    public async Task<Producto> AddProductAsync(Producto producto)
    {
        await _productoRepository.AddAsync(producto);
        return producto;
    }

    public async Task<Producto> UpdateProductAsync(Producto producto)
    {
        await _productoRepository.UpdateAsync(producto);
        return producto;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _productoRepository.GetByIdAsync(id);
        if (existingProduct == null) return false;

        await _productoRepository.DeleteAsync(id);
        return true;
    }
}
