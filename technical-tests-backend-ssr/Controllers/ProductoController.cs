using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productService"></param>
    /// <param name="mapper"></param>
    public ProductoController(ProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(_mapper.Map<IEnumerable<ProductoDTO>>(products));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDTO>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(_mapper.Map<ProductoDTO>(product));
    }


    /// <summary>
    /// Agregar un nuevo producto    
    /// </summary>
    /// <param name="productoDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ProductoDTO>> Create(ProductoDTO productoDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Producto>(productoDTO);
        var newProduct = await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, _mapper.Map<ProductoDTO>(newProduct));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductoDTO>> Update(int id, [FromBody] ProductoDTO productoDTO)
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound($"No se encontró el producto con ID {id}.");
        }

        _mapper.Map(productoDTO, product);
        await _productService.UpdateProductAsync(product);

        var updatedProductDTO = _mapper.Map<ProductoDTO>(product);
        return Ok(updatedProductDTO); // Retornar el producto actualizado
    }

    /// <summary>
    /// Eliminar un producto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
