using Logistica.Application;
using Logistica.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Logistica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly IAssignCourierUseCase _assignCourierUseCase;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ILogger<DeliveriesController> _logger; // Observabilidad

    public DeliveriesController(
        IAssignCourierUseCase assignCourierUseCase,
        IDeliveryRepository deliveryRepository,
        ILogger<DeliveriesController> logger)
    {
        _assignCourierUseCase = assignCourierUseCase;
        _deliveryRepository = deliveryRepository;
        _logger = logger;
    }

    [HttpPost("{deliveryId}/assign-courier")]
    public async Task<IActionResult> AssignCourier(Guid deliveryId, [FromBody] AssignCourierRequest request)
    {
        _logger.LogInformation("Iniciando asignación de repartidor. DeliveryId: {DeliveryId}, CourierId: {CourierId}", deliveryId, request.CourierId);

        try
        {
            var command = new AssignCourierCommand(
                new DeliveryId(deliveryId), 
                new CourierId(request.CourierId));

            await _assignCourierUseCase.ExecuteAsync(command);
            
            _logger.LogInformation("Asignación exitosa. DeliveryId: {DeliveryId}", deliveryId);
            return Ok(new { Message = "Repartidor asignado exitosamente." });
        }
        catch (DeliveryNotFoundException ex)
        {
            _logger.LogWarning("Entrega no encontrada. DeliveryId: {DeliveryId}", deliveryId);
            return NotFound(new { Error = ex.Message });
        }
        catch (CourierNotAvailableException ex)
        {
            _logger.LogWarning("Repartidor no disponible. CourierId: {CourierId}", request.CourierId);
            return BadRequest(new { Error = ex.Message });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Regla de negocio violada. DeliveryId: {DeliveryId}. Error: {Error}", deliveryId, ex.Message);
            return Conflict(new { Error = ex.Message }); // 409 Conflict
        }
        catch (Exception ex)
        {
            // El log de error NO debe exponer datos sensibles del cliente, solo IDs técnicos
            _logger.LogError(ex, "Error interno procesando la asignación. DeliveryId: {DeliveryId}", deliveryId);
            return StatusCode(500, new { Error = "Error interno del servidor." });
        }
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDelivery()
    {
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle Falsa 123"));
        await _deliveryRepository.SaveAsync(delivery);
        _logger.LogInformation("Entrega de prueba creada. DeliveryId: {DeliveryId}", delivery.Id.Value);
        return Ok(new { Message = "Entrega creada.", DeliveryId = delivery.Id.Value });
    }
}
