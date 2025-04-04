### Tecnologías Aplicados al Proyecto de Programación Concurrente y Paralela. 
#### 🚗 Venta y Distribución de Autos
#### SSR Motors – Concesionaria

```markdown
## NET 9 - WebApi
El proyecto se desarrolló utilizando **NET 9**, una versión de **LTS (Long-Term Support)** que ofrece:
- **Alto rendimiento** y menor consumo de recursos.
- **Soporte mejorado para contenedores** y despliegue en la nube.
- **http://localhost:5156/swagger/index.html

## Patrón Repository
El **Patrón Repository** se implementa para **separar la lógica de acceso a datos** de la lógica de negocio.  

### Ejemplo de `IProductoRepository.cs`

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(int id);
    Task<Producto> AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(int id);
}
```

### Implementación en `ProductoRepository.cs` con métodos asinconrónicos

```csharp
public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _context;

    public ProductoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync() => await _context.Productos.ToListAsync();

    public async Task<Producto?> GetByIdAsync(int id) => await _context.Productos.FindAsync(id);

    public async Task<Producto> AddAsync(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task UpdateAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var producto = await GetByIdAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }
    }
}

```

### Patrón Service

El **Patrón Service** encapsula la lógica de negocio en **una capa intermedia** entre los controladores y los repositorios.  

### Implementación en `ProductoService.cs`

```csharp
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<Producto>> GetAllProductsAsync() => await _productoRepository.GetAllAsync();

    public async Task<Producto?> GetProductByIdAsync(int id) => await _productoRepository.GetByIdAsync(id);

    public async Task<Producto> AddProductAsync(Producto producto) => await _productoRepository.AddAsync(producto);

    public async Task UpdateProductAsync(Producto producto) => await _productoRepository.UpdateAsync(producto);

    public async Task DeleteProductAsync(int id) => await _productoRepository.DeleteAsync(id);
}

```
-----
###  AutoMapper

Se usa **AutoMapper** para **mapear entidades y DTOs** automáticamente, reduciendo la cantidad de código manual de conversión.

### Instalación:

```sh
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

```

### Configuración en `AutoMapperProfile.cs`

```csharp
using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Producto, ProductoDTO>().ReverseMap();
    }
}

```

### Entity Framework Core (Base de datos: MySQL)

Se usó **Entity Framework Core** como ORM para interactuar con la base de datos **MySQL**.

### Instalación:

```sh
dotnet add package MySql.EntityFrameworkCore

```

### Configuración de `ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Producto> Productos { get; set; }
}

```

### Configuración en `appsettings.json`

```json
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=MiBaseDeDatos;user=root;password=********"
}

```

###  Inyección de Dependencias (Dependency Injection)

Se usa **Inyección de Dependencias** para registrar y gestionar servicios en la aplicación.

### Registro en `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();

```

----------

### Resumen: technical tests

Tecnología/Patrón

**.NET 6+**

>Mejor rendimiento y compatibilidad.
>
>http://localhost:5156/swagger/index.html

**Patrón Repository**

>Desacopla acceso a datos y facilita pruebas.

**Patrón Service**

>Lógica de negocio separada y reutilizable.

**AutoMapper**

>Conversión automática entre entidades y DTOs.

**Entity Framework Core**

>Simplifica interacciones con MySQL.

**Inyección de Dependencias**

>Gestión centralizada de dependencias.

