using Domain;

namespace Application;

public enum AssignCourierError
{
    None,
    DeliveryNotFound,
    AlreadyAssigned,
    InvalidState,
    CourierUnavailable,
    RouteSaturated,
    AvailabilityCheckFailed
}

public sealed record AssignCourierCommand(Guid DeliveryId, Guid CourierId, string RequestedBy);

public sealed record AssignCourierResult(bool Success, AssignCourierError Error, string? DeliveryStatus);

public sealed class AssignCourierUseCase
{
    private readonly IDeliveryRepository _repository;
    private readonly ICourierAvailabilityPort _availability;
    private readonly IAuditLog _audit;
    private readonly IEventPublisher _publisher;
    private readonly IClock _clock;

    public AssignCourierUseCase(
        IDeliveryRepository repository,
        ICourierAvailabilityPort availability,
        IAuditLog audit,
        IEventPublisher publisher,
        IClock clock)
    {
        _repository = repository;
        _availability = availability;
        _audit = audit;
        _publisher = publisher;
        _clock = clock;
    }

    public async Task<AssignCourierResult> ExecuteAsync(AssignCourierCommand command)
    {
        var deliveryId = new DeliveryId(command.DeliveryId);
        var courierId = new CourierId(command.CourierId);

        // 1. Cargar la entrega
        var delivery = await _repository.GetAsync(deliveryId);
        if (delivery is null)
            return Fail(AssignCourierError.DeliveryNotFound, null);

        // 2. Verificar que no tenga repartidor activo
        if (delivery.CourierId is not null)
            return Fail(AssignCourierError.AlreadyAssigned, delivery);

        // 3. Consultar disponibilidad mediante un puerto
        CourierAvailability availability;
        try
        {
            availability = await _availability.CheckAsync(courierId);
        }
        catch (Exception)
        {
            return Fail(AssignCourierError.AvailabilityCheckFailed, delivery);
        }

        // INV-04 e INV-03
        if (!availability.IsAvailable)
            return Fail(AssignCourierError.CourierUnavailable, delivery);
        if (!availability.HasRouteCapacity)
            return Fail(AssignCourierError.RouteSaturated, delivery);

        // 4. El dominio protege sus invariantes
        try
        {
            delivery.AssignCourier(courierId);
        }
        catch (DomainException ex)
        {
            return Fail(ex.Error == DomainError.AlreadyAssigned
                ? AssignCourierError.AlreadyAssigned
                : AssignCourierError.InvalidState, delivery);
        }

        // 5. Guardar
        await _repository.SaveAsync(delivery);

        // 6. Registrar y publicar el resultado (INV-07)
        var now = _clock.UtcNow;
        await _audit.RecordAsync(deliveryId, courierId, command.RequestedBy, now);
        await _publisher.PublishAsync(new DeliveryAssigned(deliveryId, courierId, now));

        return new AssignCourierResult(true, AssignCourierError.None, delivery.Status.ToString());
    }

    private static AssignCourierResult Fail(AssignCourierError error, Delivery? delivery) =>
        new(false, error, delivery?.Status.ToString());
}
