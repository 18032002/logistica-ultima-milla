using Logistica.Domain;

namespace Logistica.Application;

public interface IAssignCourierUseCase
{
    Task ExecuteAsync(AssignCourierCommand command, CancellationToken ct = default);
}

public class AssignCourierUseCase : IAssignCourierUseCase
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICourierAvailabilityPort _courierAvailability;
    private readonly IEventPublisher _eventPublisher;

    public AssignCourierUseCase(
        IDeliveryRepository deliveryRepository,
        ICourierAvailabilityPort courierAvailability,
        IEventPublisher eventPublisher)
    {
        _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
        _courierAvailability = courierAvailability ?? throw new ArgumentNullException(nameof(courierAvailability));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public async Task ExecuteAsync(AssignCourierCommand command, CancellationToken ct = default)
    {
        // 1. Cargar la entrega
        var delivery = await _deliveryRepository.GetByIdAsync(command.DeliveryId, ct)
            ?? throw new DeliveryNotFoundException(command.DeliveryId);

        // 2. Consultar disponibilidad del repartidor mediante el puerto
        var isAvailable = await _courierAvailability.IsAvailableAsync(command.CourierId, ct);
        if (!isAvailable)
            throw new CourierNotAvailableException(command.CourierId);

        // 3. Ejecutar la regla del dominio (protege la invariante)
        delivery.AssignCourier(command.CourierId);

        // 4. Guardar en el repositorio
        await _deliveryRepository.SaveAsync(delivery, ct);

        // 5. Publicar el evento
        await _eventPublisher.PublishAsync(new DeliveryAssignedEvent(delivery.Id, command.CourierId), ct);
    }
}
