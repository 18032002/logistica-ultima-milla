namespace Logistica.Domain;

public interface IDeliveryRepository
{
    Task<Delivery?> GetByIdAsync(DeliveryId id, CancellationToken ct = default);
    Task SaveAsync(Delivery delivery, CancellationToken ct = default);
}
