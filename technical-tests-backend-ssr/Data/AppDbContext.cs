using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Data;
public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("Vehiculos");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(v => v.Modelo)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(v => v.Anio)
                .IsRequired();
            entity.Property(v => v.Precio)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        });
        AppDbContext.Seed(modelBuilder);
    }
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasData(
            new Cliente { Id = 1, Nombre = "Santiago", Apellido = "Cuevas", Email = "santiagobcuevas14@gmail.com", Telefono = "1161970490" },
            new Cliente { Id = 2, Nombre = "Lucía", Apellido = "Pérez", Email = "lucia.perez@email.com", Telefono = "1134567890" },
            new Cliente { Id = 3, Nombre = "Martín", Apellido = "Gómez", Email = "martin.gomez@email.com", Telefono = "1145678901" },
            new Cliente { Id = 4, Nombre = "Camila", Apellido = "Rodríguez", Email = "camila.rodriguez@email.com", Telefono = "1156789012" },
            new Cliente { Id = 5, Nombre = "Julián", Apellido = "Fernández", Email = "julian.fernandez@email.com", Telefono = "1167890123" },
            new Cliente { Id = 6, Nombre = "Sofía", Apellido = "López", Email = "sofia.lopez@email.com", Telefono = "1178901234" },
            new Cliente { Id = 7, Nombre = "Mateo", Apellido = "Díaz", Email = "mateo.diaz@email.com", Telefono = "1189012345" },
            new Cliente { Id = 8, Nombre = "Valentina", Apellido = "Martínez", Email = "valentina.martinez@email.com", Telefono = "1190123456" },
            new Cliente { Id = 9, Nombre = "Tomás", Apellido = "Sosa", Email = "tomas.sosa@email.com", Telefono = "1101234567" },
            new Cliente { Id = 10, Nombre = "Agustina", Apellido = "Herrera", Email = "agustina.herrera@email.com", Telefono = "1112345678" }
        );

        modelBuilder.Entity<Vehiculo>().HasData(
            new Vehiculo { Id = 1, Marca = "Toyota", Modelo = "Corolla", Anio = 2020, Precio = 5100000, Stock = 10 },
            new Vehiculo { Id = 2, Marca = "Ford", Modelo = "Focus", Anio = 2019, Precio = 6300000, Stock = 8 },
            new Vehiculo { Id = 3, Marca = "Chevrolet", Modelo = "Cruze", Anio = 2021, Precio = 5500000, Stock = 12 },
            new Vehiculo { Id = 4, Marca = "Honda", Modelo = "Civic", Anio = 2022, Precio = 7000000, Stock = 5 },
            new Vehiculo { Id = 5, Marca = "Nissan", Modelo = "Sentra", Anio = 2018, Precio = 5200000, Stock = 15 },
            new Vehiculo { Id = 6, Marca = "Hyundai", Modelo = "Elantra", Anio = 2020, Precio = 5600000, Stock = 7 },
            new Vehiculo { Id = 7, Marca = "Volkswagen", Modelo = "Golf", Anio = 2021, Precio = 6500000, Stock = 6 },
            new Vehiculo { Id = 8, Marca = "BMW", Modelo = "Serie 3", Anio = 2019, Precio = 8000000, Stock = 4 },
            new Vehiculo { Id = 9, Marca = "Audi", Modelo = "A4", Anio = 2022, Precio = 9000000, Stock = 3 },
            new Vehiculo { Id = 10, Marca = "Mercedes-Benz", Modelo = "Clase C", Anio = 2020, Precio = 8500000, Stock = 2 }
        );
    }
}