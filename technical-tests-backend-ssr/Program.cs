using technical_tests_backend_ssr.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using technical_tests_backend_ssr.Repositories;
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteDTOValidator>();

//Clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();


//Vehiculos
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<VehiculoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API SSR Motors – Concesionaria",
        Version = "v1",
        Description = "Venta y Distribución de Autos\nTrabajo práctico realizado para programación paralela y concurrente",
        Contact = new OpenApiContact
        {
            Name = "Santiago Baldini Cuevas",
            Email = "santiagobcuevas14@gmail.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Habilitar Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSR Motors API v1");
    });
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Aplica las migraciones pendientes
    
}

app.Run();
