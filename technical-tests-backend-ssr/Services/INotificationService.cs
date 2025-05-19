using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Services;

/// <summary>
/// Interface for notification service that handles email notifications
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends a notification for a new sale
    /// </summary>
    /// <param name="venta">The sale to notify about</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendSaleNotificationAsync(Venta venta);

    /// <summary>
    /// Sends a notification for a new client registration
    /// </summary>
    /// <param name="cliente">The client to notify about</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendNewClientNotificationAsync(Cliente cliente);

    /// <summary>
    /// Sends a notification for a new vehicle registration
    /// </summary>
    /// <param name="vehiculo">The vehicle to notify about</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendNewVehicleNotificationAsync(Vehiculo vehiculo);
} 