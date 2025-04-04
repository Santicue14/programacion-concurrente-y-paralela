using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Productos");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Precio)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(p => p.Stock)
                .IsRequired();
        });
        AppDbContext.Seed(modelBuilder);
    }
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>().HasData(
        new Producto { Id = 1, Nombre = "Cuaderno A4", Precio = 100m, Stock = 10 },
        new Producto { Id = 2, Nombre = "Lápiz HB", Precio = 200m, Stock = 20 },
        new Producto { Id = 3, Nombre = "Bolígrafo Azul", Precio = 300m, Stock = 30 },
        new Producto { Id = 4, Nombre = "Carpeta Cartón", Precio = 400m, Stock = 40 },
        new Producto { Id = 5, Nombre = "Regla 30 cm", Precio = 500m, Stock = 50 },
        new Producto { Id = 6, Nombre = "Marcador Amarillo", Precio = 600m, Stock = 60 },
        new Producto { Id = 7, Nombre = "Tijeras", Precio = 700m, Stock = 70 },
        new Producto { Id = 8, Nombre = "Pegamento Líquido", Precio = 800m, Stock = 80 },
        new Producto { Id = 9, Nombre = "Papel Bond", Precio = 900m, Stock = 90 },
        new Producto { Id = 10, Nombre = "Carpeta de Anillas", Precio = 1000m, Stock = 100 }
        );
    }
}