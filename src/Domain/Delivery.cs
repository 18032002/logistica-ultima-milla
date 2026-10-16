namespace Domain;

public sealed class Delivery
{
    public DeliveryId Id { get; }
    public DeliveryAddress Address { get; }
    public DeliveryStatus Status { get; private set; }
    public CourierId? CourierId { get; private set; }

    public Delivery(DeliveryId id, DeliveryAddress address)
    {
        Id = id;
        Address = address;
        Status = DeliveryStatus.ListaParaDespacho;
    }

    // INV-01 e INV-02 (y protege INV-05)
    public void AssignCourier(CourierId courierId)
    {
        if (CourierId is not null)
            throw new DomainException(DomainError.AlreadyAssigned,
                "La entrega ya tiene repartidor.");

        if (Status != DeliveryStatus.ListaParaDespacho)
            throw new DomainException(DomainError.InvalidState,
                $"No se puede asignar en estado {Status}.");

        CourierId = courierId;
        Status = DeliveryStatus.Asignada;
    }

    public void StartRoute()
    {
        if (Status != DeliveryStatus.Asignada)
            throw new DomainException(DomainError.InvalidState,
                $"No se puede iniciar ruta en estado {Status}.");
        Status = DeliveryStatus.EnRuta;
    }

    // INV-06
    public void Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException(DomainError.ReasonRequired,
                "La cancelación requiere un motivo.");

        if (Status is not (DeliveryStatus.ListaParaDespacho or DeliveryStatus.Asignada))
            throw new DomainException(DomainError.InvalidState,
                $"No se puede cancelar en estado {Status}.");

        Status = DeliveryStatus.Cancelada;
    }
}
