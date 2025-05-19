using technical_tests_backend_ssr.Models;
using System.Diagnostics;
using System.Threading;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Implementation of notification service that handles email notifications
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly Semaphore _semaphore; //Creamos un semáforo para limitar el número de emails concurrentes
    private readonly int _maxConcurrentEmails;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger; //El logger es para que se pueda imprimir en la consola
        _maxConcurrentEmails = 5; // Número máximo de emails concurrentes
        _semaphore = new Semaphore(_maxConcurrentEmails, _maxConcurrentEmails); //Creamos el semáforo con el número máximo de emails concurrentes
    }

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
} 