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

    /// <summary>
    /// Sends a notification for a maintenance reminder
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendMaintenanceReminderAsync(Venta venta);

    /// <summary>
    /// Sends a notification for a maintenance service request
    /// </summary>
    /// <param name="tipoMantenimiento">Type of maintenance service</param>
    /// <param name="fechaProgramada">Scheduled date for maintenance</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendMaintenanceServiceRequestAsync(string tipoMantenimiento, DateTime fechaProgramada);

    /// <summary>
    /// Sends a notification for a warranty claim
    /// </summary>
    /// <param name="descripcionProblema">Description of the warranty issue</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendWarrantyClaimNotificationAsync(string descripcionProblema);

    /// <summary>
    /// Sends a notification for a customer complaint
    /// </summary>
    /// <param name="descripcionReclamo">Description of the complaint</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendCustomerComplaintNotificationAsync(string descripcionReclamo);
} 