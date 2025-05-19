using technical_tests_backend_ssr.Models;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using technical_tests_backend_ssr.Repositories;
namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Implementation of notification service that handles email notifications
/// </summary>
public class NotificationService : INotificationService, IHostedService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly Semaphore _semaphore; //Creamos un semáforo para limitar el número de emails concurrentes
    private readonly int _maxConcurrentEmails;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>   
    /// Intervalo de tiempo para revisar notificaciones de mantenimiento
    /// </summary>
    private readonly TimeSpan _checkInterval = TimeSpan.FromDays(1); // Revisar diariamente

    /// <summary>
    /// Intervalo de tiempo para enviar notificaciones de mantenimiento
    /// </summary>
    private readonly TimeSpan _maintenanceInterval = TimeSpan.FromDays(180); // 6 meses
    private Timer? _maintenanceTimer;

    public NotificationService(
        ILogger<NotificationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _maxConcurrentEmails = 5;
        _semaphore = new Semaphore(_maxConcurrentEmails, _maxConcurrentEmails);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Servicio de notificaciones iniciado");
        _maintenanceTimer = new Timer(CheckMaintenanceNotifications, null, TimeSpan.Zero, _checkInterval);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Servicio de notificaciones detenido");
        _maintenanceTimer?.Dispose();
        return Task.CompletedTask;
    }

    private async void CheckMaintenanceNotifications(object? state)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var ventaRepository = scope.ServiceProvider.GetRequiredService<IVentaRepository>();
            var ventas = await ventaRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            foreach (var venta in ventas)
            {
                var timeSinceSale = now - venta.Fecha;
                
                // Verificar si han pasado 6 meses desde la venta
                if (timeSinceSale >= _maintenanceInterval)
                {
                    // Calcular cuántos intervalos de 6 meses han pasado
                    var intervals = (int)(timeSinceSale.TotalDays / _maintenanceInterval.TotalDays);
                    
                    // Solo enviar notificación si es un múltiplo exacto de 6 meses
                    if (timeSinceSale.TotalDays % _maintenanceInterval.TotalDays < 1)
                    {
                        await SendMaintenanceReminderAsync(venta);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar notificaciones de mantenimiento");
        }
    }

    public async Task SendMaintenanceReminderAsync(Venta venta)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de recordatorio de mantenimiento enviado a {venta.Cliente?.Email} para el vehículo {venta.Vehiculo?.Modelo?.Nombre}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de mantenimiento: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de mantenimiento");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de venta
    /// </summary>
    /// <param name="venta">La venta a notificar</param>
    public async Task SendSaleNotificationAsync(Venta venta)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de venta enviado a {venta.Cliente?.Email} por la venta del vehículo {venta.Vehiculo?.Modelo?.Nombre}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de venta: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de venta");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de nuevo cliente
    /// </summary>
    /// <param name="cliente">El cliente a notificar</param>
    public async Task SendNewClientNotificationAsync(Cliente cliente)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de bienvenida enviado a {cliente.Email}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de bienvenida: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de bienvenida");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de nuevo vehículo
    /// </summary>
    /// <param name="vehiculo">El vehículo a notificar</param>
    public async Task SendNewVehicleNotificationAsync(Vehiculo vehiculo)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de nuevo vehículo enviado: {vehiculo.Modelo?.Marca?.Nombre} {vehiculo.Modelo?.Nombre}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de nuevo vehículo: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de nuevo vehículo");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de solicitud de mantenimiento
    /// </summary>
    /// <param name="tipoMantenimiento">El tipo de mantenimiento</param>
    /// <param name="fechaProgramada">La fecha programada</param>
    public async Task SendMaintenanceServiceRequestAsync(string tipoMantenimiento, DateTime fechaProgramada)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de solicitud de mantenimiento enviado. " +
                                 $"Tipo: {tipoMantenimiento}, Fecha programada: {fechaProgramada:dd/MM/yyyy}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de solicitud de mantenimiento: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de solicitud de mantenimiento");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de reclamo de garantía
    /// </summary>
    /// <param name="descripcionProblema">La descripción del problema</param>
    public async Task SendWarrantyClaimNotificationAsync(string descripcionProblema)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de reclamo de garantía enviado. " +
                                 $"Problema: {descripcionProblema}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de reclamo de garantía: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de reclamo de garantía");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Envia una notificación de reclamo de cliente
    /// </summary>
    /// <param name="descripcionReclamo">La descripción del reclamo</param>
    public async Task SendCustomerComplaintNotificationAsync(string descripcionReclamo)
    {
        _semaphore.WaitOne();
        try
        {
            var cronometro = Stopwatch.StartNew();
            
            // Simulamos el envío de email
            await Task.Delay(100); // Simulación de tiempo de envío
            
            _logger.LogInformation($"Email de reclamo de cliente enviado. " +
                                 $"Reclamo: {descripcionReclamo}");
            
            cronometro.Stop();
            _logger.LogInformation($"Tiempo de envío de email de reclamo de cliente: {cronometro.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de reclamo de cliente");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }
} 