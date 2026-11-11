using Logistica.Domain;

namespace Logistica.Infrastructure;

public class InMemoryDeliveryRepository : IDeliveryRepository
{
    private readonly Dictionary<DeliveryId, Delivery> _deliveries = new();

    public Task<Delivery?> GetByIdAsync(DeliveryId id, CancellationToken ct = default)
    {
        _deliveries.TryGetValue(id, out var delivery);
        return Task.FromResult(delivery);
    }

    public Task SaveAsync(Delivery delivery, CancellationToken ct = default)
    {
        _deliveries[delivery.Id] = delivery;
        return Task.CompletedTask;
    }
}
