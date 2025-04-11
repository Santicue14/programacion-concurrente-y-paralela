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

            entity.Property(v => v.Anio)
                .IsRequired();

            entity.Property(v => v.Precio)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(v => v.Stock)
                .IsRequired()
                .HasDefaultValue(0);

            entity.HasOne(v => v.Modelo)
                .WithMany(m => m.Vehiculos)
                .HasForeignKey(v => v.ModeloId)
                .OnDelete(DeleteBehavior.Cascade);


        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marcas");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.ToTable("Modelos");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasOne(m => m.Marca)
                .WithMany(m => m.Modelos)
                .HasForeignKey(m => m.MarcaId)
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<Marca>().HasData(
            new Marca { Id = 1, Nombre = "Toyota" },
            new Marca { Id = 2, Nombre = "Ford" },
            new Marca { Id = 3, Nombre = "Chevrolet" },
            new Marca { Id = 4, Nombre = "Honda" },
            new Marca { Id = 5, Nombre = "Nissan" },
            new Marca { Id = 6, Nombre = "Hyundai" },
            new Marca { Id = 7, Nombre = "Volkswagen" },
            new Marca { Id = 8, Nombre = "BMW" },
            new Marca { Id = 9, Nombre = "Audi" },
            new Marca { Id = 10, Nombre = "Mercedes-Benz" },
            new Marca { Id = 11, Nombre = "Renault" },
            new Marca { Id = 12, Nombre = "Peugeot" },
            new Marca { Id = 13, Nombre = "Fiat" }
        );

        modelBuilder.Entity<Modelo>().HasData(
            new Modelo { Id = 1, MarcaId = 1, Nombre = "Corolla" },
            new Modelo { Id = 2, MarcaId = 2, Nombre = "Focus" },
            new Modelo { Id = 3, MarcaId = 3, Nombre = "Cruze" },
            new Modelo { Id = 4, MarcaId = 4, Nombre = "Civic" },
            new Modelo { Id = 5, MarcaId = 5, Nombre = "Sentra" },
            new Modelo { Id = 6, MarcaId = 6, Nombre = "Elantra" },
            new Modelo { Id = 7, MarcaId = 7, Nombre = "Golf" },
            new Modelo { Id = 8, MarcaId = 8, Nombre = "Serie 3" },
            new Modelo { Id = 9, MarcaId = 9, Nombre = "A4" },
            new Modelo { Id = 10, MarcaId = 10, Nombre = "Clase C" },
            new Modelo { Id = 11, MarcaId = 11, Nombre = "Kangoo" },
            new Modelo { Id = 12, MarcaId = 12, Nombre = "208" },
            new Modelo { Id = 13, MarcaId = 13, Nombre = "Cronos" }
        );

        modelBuilder.Entity<Vehiculo>().HasData(
            new Vehiculo { Id = 1, ModeloId = 1, Anio = 2020, Precio = 5100000, Stock = 10 },
            new Vehiculo { Id = 2, ModeloId = 2, Anio = 2019, Precio = 6300000, Stock = 8 },
            new Vehiculo { Id = 3, ModeloId = 3, Anio = 2021, Precio = 5500000, Stock = 12 },
            new Vehiculo { Id = 4, ModeloId = 4, Anio = 2022, Precio = 7000000, Stock = 5 },
            new Vehiculo { Id = 5, ModeloId = 5, Anio = 2018, Precio = 5200000, Stock = 15 },
            new Vehiculo { Id = 6, ModeloId = 6, Anio = 2020, Precio = 5600000, Stock = 7 },
            new Vehiculo { Id = 7, ModeloId = 7, Anio = 2021, Precio = 6500000, Stock = 6 },
            new Vehiculo { Id = 8, ModeloId = 8, Anio = 2019, Precio = 8000000, Stock = 4 },
            new Vehiculo { Id = 9, ModeloId = 9, Anio = 2022, Precio = 9000000, Stock = 3 },
            new Vehiculo { Id = 10, ModeloId = 10, Anio = 2020, Precio = 8500000, Stock = 2 },
            new Vehiculo { Id = 11, ModeloId = 11, Anio = 2021, Precio = 5100000, Stock = 10 },
            new Vehiculo { Id = 12, ModeloId = 12, Anio = 2022, Precio = 6200000, Stock = 9 },
            new Vehiculo { Id = 13, ModeloId = 13, Anio = 2023, Precio = 5900000, Stock = 11 }
        );
    }
}