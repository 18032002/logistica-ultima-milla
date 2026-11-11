namespace Logistica.Domain;

public class Delivery
{
    public DeliveryId Id { get; private set; }
    public DeliveryAddress Address { get; private set; }
    public CourierId? CourierId { get; private set; }
    public DeliveryStatus Status { get; private set; }

    // Constructor para crear una nueva entrega
    public Delivery(DeliveryId id, DeliveryAddress address)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Status = DeliveryStatus.Pending;
    }

    // Método que protege la invariante principal de la Actividad 3
    public void AssignCourier(CourierId courierId)
    {
        if (Status != DeliveryStatus.Pending)
            throw new DomainException($"No se puede asignar un repartidor a una entrega en estado {Status} (INV-03).");

        if (CourierId is not null)
            throw new DomainException("La entrega ya tiene un repartidor asignado (INV-01).");

        CourierId = courierId ?? throw new ArgumentNullException(nameof(courierId));
        Status = DeliveryStatus.Assigned;
    }
}
