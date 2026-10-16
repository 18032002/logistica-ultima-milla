using Application;
using Domain;

namespace Infrastructure;

public sealed class InMemoryDeliveryRepository : IDeliveryRepository
{
    private readonly Dictionary<Guid, Delivery> _store = new();

    public Task<Delivery?> GetAsync(DeliveryId id) =>
        Task.FromResult(_store.TryGetValue(id.Value, out var d) ? d : null);

    public Task SaveAsync(Delivery delivery)
    {
        _store[delivery.Id.Value] = delivery;
        return Task.CompletedTask;
    }
}

public sealed class FakeCourierAvailabilityAdapter : ICourierAvailabilityPort
{
    private readonly Func<CourierId, CourierAvailability> _rule;

    public FakeCourierAvailabilityAdapter(Func<CourierId, CourierAvailability>? rule = null)
    {
        _rule = rule ?? (_ => new CourierAvailability(true, true));
    }

    public Task<CourierAvailability> CheckAsync(CourierId courierId) =>
        Task.FromResult(_rule(courierId));
}

public sealed class ConsoleAuditLog : IAuditLog
{
    public Task RecordAsync(DeliveryId deliveryId, CourierId courierId, string requestedBy, DateTime atUtc)
    {
        Console.WriteLine($"[AUDIT] {atUtc:O} {requestedBy} asignó {courierId.Value} a {deliveryId.Value}");
        return Task.CompletedTask;
    }
}

public sealed class InMemoryEventPublisher : IEventPublisher
{
    public List<DeliveryAssigned> Published { get; } = new();

    public Task PublishAsync(DeliveryAssigned evt)
    {
        Published.Add(evt);
        return Task.CompletedTask;
    }
}

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
