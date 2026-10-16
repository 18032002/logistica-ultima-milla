using Domain;

namespace Application;

public interface IDeliveryRepository
{
    Task<Delivery?> GetAsync(DeliveryId id);
    Task SaveAsync(Delivery delivery);
}

public sealed record CourierAvailability(bool IsAvailable, bool HasRouteCapacity);

public interface ICourierAvailabilityPort
{
    Task<CourierAvailability> CheckAsync(CourierId courierId);
}

public interface IAuditLog
{
    Task RecordAsync(DeliveryId deliveryId, CourierId courierId, string requestedBy, DateTime atUtc);
}

public sealed record DeliveryAssigned(DeliveryId DeliveryId, CourierId CourierId, DateTime AssignedAtUtc);

public interface IEventPublisher
{
    Task PublishAsync(DeliveryAssigned evt);
}

public interface IClock
{
    DateTime UtcNow { get; }
}
