using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace technical_tests_backend_ssr.Data;
/// <summary>
/// Clase que representa el contexto de la base de datos.
/// </summary>
public class AppDbContext : DbContext
{

    /// <summary>
    /// Constructor de la clase AppDbContext.
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Representa la tabla de clientes en la base de datos.
    /// </summary>
    public DbSet<Cliente> Clientes { get; set; }
    /// <summary>
    /// Representa la tabla de vehículos en la base de datos.
    /// </summary>
    public DbSet<Vehiculo> Vehiculos { get; set; }

    /// <summary>
    /// Representa la tabla de marcas en la base de datos.
    /// </summary>
    public DbSet<Marca> Marcas { get; set; }

    /// <summary>
    /// Representa la tabla de modelos en la base de datos.
    /// </summary>
    public DbSet<Modelo> Modelos { get; set; }

    /// <summary>
    /// Representa la tabla de ventas en la base de datos.
    /// </summary>
    public DbSet<Venta> Ventas { get; set; }

    /// <summary>
    /// Método para configurar el modelo de la base de datos.
    /// </summary>
    /// <param name="modelBuilder"></param>
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


    /// <summary>
    /// Método para inicializar la base de datos con datos de prueba.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Clientes existentes
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

        // Marcas existentes
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

        // Modelos existentes
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

        // Generar más vehículos
        var vehiculos = new List<Vehiculo>();
        var random = new Random();
        var id = 1;

        for (int modeloId = 1; modeloId <= 13; modeloId++)
        {
            for (int i = 0; i < 50; i++) // 50 vehículos por modelo
            {
                vehiculos.Add(new Vehiculo
                {
                    Id = id++,
                    ModeloId = modeloId,
                    Anio = random.Next(2018, 2024),
                    Precio = random.Next(5000000, 15000000),
                    Stock = random.Next(1, 20)
                });
            }
        }

        modelBuilder.Entity<Vehiculo>().HasData(vehiculos);

        // Generar ventas
        var ventas = new List<Venta>();
        var fechaInicio = new DateTime(2023, 1, 1);
        var fechaFin = DateTime.Now;

        for (int i = 1; i <= 1000; i++)
        {
            var clienteId = random.Next(1, 11); // 10 clientes
            var vehiculoId = random.Next(1, vehiculos.Count + 1);
            var vehiculo = vehiculos.First(v => v.Id == vehiculoId);
            var fecha = fechaInicio.AddDays(random.Next((fechaFin - fechaInicio).Days));

            ventas.Add(new Venta
            {
                Id = i,
                ClienteId = clienteId,
                VehiculoId = vehiculoId,
                Fecha = fecha,
                Total = vehiculo.Precio
            });
        }

        modelBuilder.Entity<Venta>().HasData(ventas);
    }
}