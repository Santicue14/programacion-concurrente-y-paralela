using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using technical_tests_backend_ssr.Models.Enums;

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
    /// Representa la tabla de servicios de posventa en la base de datos.
    /// </summary>
    public DbSet<ServicioPosventa> ServiciosPosventa { get; set; }


    /// <summary>
    /// Representa la tabla de tipos de servicio en la base de datos.
    /// </summary>
    public DbSet<TipoServicio> TiposServicio { get; set; }

    /// <summary>
    /// Representa la tabla de usuarios en la base de datos.
    /// </summary>
    public DbSet<Usuario> Usuarios { get; set; }

    /// Método para configurar el modelo de la base de datos.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            entity.HasMany(c => c.ServiciosPosventa)
                .WithOne(s => s.Cliente)
                .HasForeignKey(s => s.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
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

        // Configuración de TiposServicio
        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.ToTable("TiposServicio");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Descripcion).IsRequired().HasMaxLength(500);
            entity.Property(t => t.Activo).IsRequired();
            entity.Property(t => t.FechaCreacion).IsRequired();

            // Relación con ServiciosPosventa
            entity.HasMany(t => t.ServiciosPosventa)
                .WithOne(s => s.TipoServicio)
                .HasForeignKey(s => s.TipoServicioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de ServiciosPosventa
        modelBuilder.Entity<ServicioPosventa>(entity =>
        {
            entity.ToTable("ServiciosPosventa");
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Cliente)
                .WithMany(c => c.ServiciosPosventa)
                .HasForeignKey(s => s.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(s => s.Descripcion).IsRequired().HasMaxLength(500);
            entity.Property(s => s.FechaSolicitud).IsRequired();
            entity.Property(s => s.Estado).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Observaciones).HasMaxLength(1000);


            // Relación con TipoServicio
            entity.HasOne(s => s.TipoServicio)
                .WithMany(t => t.ServiciosPosventa)
                .HasForeignKey(s => s.TipoServicioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
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

        // Modelos expandidos (al menos 4 por marca)
        var modelos = new List<Modelo>
        {
            // Toyota (ID: 1)
            new Modelo { Id = 1, MarcaId = 1, Nombre = "Corolla" },
            new Modelo { Id = 2, MarcaId = 1, Nombre = "Camry" },
            new Modelo { Id = 3, MarcaId = 1, Nombre = "RAV4" },
            new Modelo { Id = 4, MarcaId = 1, Nombre = "Hilux" },
            new Modelo { Id = 5, MarcaId = 1, Nombre = "Yaris" },
            
            // Ford (ID: 2)
            new Modelo { Id = 6, MarcaId = 2, Nombre = "Focus" },
            new Modelo { Id = 7, MarcaId = 2, Nombre = "Fiesta" },
            new Modelo { Id = 8, MarcaId = 2, Nombre = "Mustang" },
            new Modelo { Id = 9, MarcaId = 2, Nombre = "Ranger" },
            new Modelo { Id = 10, MarcaId = 2, Nombre = "Ecosport" },
            
            // Chevrolet (ID: 3)
            new Modelo { Id = 11, MarcaId = 3, Nombre = "Cruze" },
            new Modelo { Id = 12, MarcaId = 3, Nombre = "Onix" },
            new Modelo { Id = 13, MarcaId = 3, Nombre = "Tracker" },
            new Modelo { Id = 14, MarcaId = 3, Nombre = "S10" },
            new Modelo { Id = 15, MarcaId = 3, Nombre = "Spin" },
            
            // Honda (ID: 4)
            new Modelo { Id = 16, MarcaId = 4, Nombre = "Civic" },
            new Modelo { Id = 17, MarcaId = 4, Nombre = "Accord" },
            new Modelo { Id = 18, MarcaId = 4, Nombre = "HR-V" },
            new Modelo { Id = 19, MarcaId = 4, Nombre = "CR-V" },
            new Modelo { Id = 20, MarcaId = 4, Nombre = "Fit" },
            
            // Nissan (ID: 5)
            new Modelo { Id = 21, MarcaId = 5, Nombre = "Sentra" },
            new Modelo { Id = 22, MarcaId = 5, Nombre = "Versa" },
            new Modelo { Id = 23, MarcaId = 5, Nombre = "X-Trail" },
            new Modelo { Id = 24, MarcaId = 5, Nombre = "Kicks" },
            new Modelo { Id = 25, MarcaId = 5, Nombre = "Frontier" },
            
            // Hyundai (ID: 6)
            new Modelo { Id = 26, MarcaId = 6, Nombre = "Elantra" },
            new Modelo { Id = 27, MarcaId = 6, Nombre = "Tucson" },
            new Modelo { Id = 28, MarcaId = 6, Nombre = "Santa Fe" },
            new Modelo { Id = 29, MarcaId = 6, Nombre = "Accent" },
            new Modelo { Id = 30, MarcaId = 6, Nombre = "Creta" },
            
            // Volkswagen (ID: 7)
            new Modelo { Id = 31, MarcaId = 7, Nombre = "Golf" },
            new Modelo { Id = 32, MarcaId = 7, Nombre = "Polo" },
            new Modelo { Id = 33, MarcaId = 7, Nombre = "Vento" },
            new Modelo { Id = 34, MarcaId = 7, Nombre = "Tiguan" },
            new Modelo { Id = 35, MarcaId = 7, Nombre = "Amarok" },
            
            // BMW (ID: 8)
            new Modelo { Id = 36, MarcaId = 8, Nombre = "Serie 3" },
            new Modelo { Id = 37, MarcaId = 8, Nombre = "Serie 5" },
            new Modelo { Id = 38, MarcaId = 8, Nombre = "X3" },
            new Modelo { Id = 39, MarcaId = 8, Nombre = "X5" },
            new Modelo { Id = 40, MarcaId = 8, Nombre = "M3" },
            
            // Audi (ID: 9)
            new Modelo { Id = 41, MarcaId = 9, Nombre = "A4" },
            new Modelo { Id = 42, MarcaId = 9, Nombre = "A3" },
            new Modelo { Id = 43, MarcaId = 9, Nombre = "Q5" },
            new Modelo { Id = 44, MarcaId = 9, Nombre = "Q3" },
            new Modelo { Id = 45, MarcaId = 9, Nombre = "A6" },
            
            // Mercedes-Benz (ID: 10)
            new Modelo { Id = 46, MarcaId = 10, Nombre = "Clase C" },
            new Modelo { Id = 47, MarcaId = 10, Nombre = "Clase A" },
            new Modelo { Id = 48, MarcaId = 10, Nombre = "Clase E" },
            new Modelo { Id = 49, MarcaId = 10, Nombre = "GLA" },
            new Modelo { Id = 50, MarcaId = 10, Nombre = "GLC" },
            
            // Renault (ID: 11)
            new Modelo { Id = 51, MarcaId = 11, Nombre = "Kangoo" },
            new Modelo { Id = 52, MarcaId = 11, Nombre = "Sandero" },
            new Modelo { Id = 53, MarcaId = 11, Nombre = "Logan" },
            new Modelo { Id = 54, MarcaId = 11, Nombre = "Duster" },
            new Modelo { Id = 55, MarcaId = 11, Nombre = "Captur" },
            
            // Peugeot (ID: 12)
            new Modelo { Id = 56, MarcaId = 12, Nombre = "208" },
            new Modelo { Id = 57, MarcaId = 12, Nombre = "308" },
            new Modelo { Id = 58, MarcaId = 12, Nombre = "2008" },
            new Modelo { Id = 59, MarcaId = 12, Nombre = "3008" },
            new Modelo { Id = 60, MarcaId = 12, Nombre = "Partner" },
            
            // Fiat (ID: 13)
            new Modelo { Id = 61, MarcaId = 13, Nombre = "Cronos" },
            new Modelo { Id = 62, MarcaId = 13, Nombre = "Argo" },
            new Modelo { Id = 63, MarcaId = 13, Nombre = "Strada" },
            new Modelo { Id = 64, MarcaId = 13, Nombre = "Toro" },
            new Modelo { Id = 65, MarcaId = 13, Nombre = "Mobi" }
        };

        modelBuilder.Entity<Modelo>().HasData(modelos);

        // Generar más vehículos
        var vehiculos = new List<Vehiculo>();
        var random = new Random();
        var id = 1;

        // Generar vehículos para cada modelo
        foreach (var modelo in modelos)
        {
            for (int i = 0; i < 20; i++) // 20 vehículos por modelo
            {
                vehiculos.Add(new Vehiculo
                {
                    Id = id++,
                    ModeloId = modelo.Id,
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

        // Tipos de Servicio
        modelBuilder.Entity<TipoServicio>().HasData(
            new TipoServicio { Id = 1, Nombre = "Mantenimiento Preventivo", Descripcion = "Servicio de mantenimiento programado", Activo = true, FechaCreacion = DateTime.UtcNow },
            new TipoServicio { Id = 2, Nombre = "Reparación Mecánica", Descripcion = "Reparación de componentes mecánicos", Activo = true, FechaCreacion = DateTime.UtcNow },
            new TipoServicio { Id = 3, Nombre = "Garantía", Descripcion = "Servicios cubiertos por garantía", Activo = true, FechaCreacion = DateTime.UtcNow },
            new TipoServicio { Id = 4, Nombre = "Diagnóstico", Descripcion = "Diagnóstico de problemas", Activo = true, FechaCreacion = DateTime.UtcNow },
            new TipoServicio { Id = 5, Nombre = "Limpieza y Detallado", Descripcion = "Servicio de limpieza y detallado", Activo = true, FechaCreacion = DateTime.UtcNow }
        );

        // Servicios Posventa
        var serviciosPosventa = new List<ServicioPosventa>();
        var estados = Enum.GetValues(typeof(EstadoServicio)).Cast<EstadoServicio>().ToArray();
        var descripciones = new[]
        {
            "Cambio de aceite y filtros",
            "Revisión de frenos",
            "Alineación y balanceo",
            "Reparación de motor",
            "Cambio de transmisión",
            "Revisión de suspensión",
            "Cambio de batería",
            "Reparación de aire acondicionado",
            "Cambio de correa de distribución",
            "Revisión de sistema eléctrico"
        };

        for (int i = 1; i <= 30; i++)
        {
            var clienteId = random.Next(1, 11); // 10 clientes
            var tipoServicioId = random.Next(1, 6); // 5 tipos de servicio
            var fechaSolicitud = DateTime.UtcNow.AddDays(-random.Next(0, 30));
            var fechaProgramada = fechaSolicitud.AddDays(random.Next(1, 15));
            var estado = estados[random.Next(estados.Length)];
            var descripcion = descripciones[random.Next(descripciones.Length)];

            serviciosPosventa.Add(new ServicioPosventa
            {
                Id = i,
                ClienteId = clienteId,
                TipoServicioId = tipoServicioId,
                Descripcion = descripcion,
                FechaSolicitud = fechaSolicitud,
                FechaProgramada = fechaProgramada,
                Estado = (int)estado
            
            });
        }

        modelBuilder.Entity<ServicioPosventa>().HasData(serviciosPosventa);
    }
}