namespace Domain;

public sealed record DeliveryId
{
    public Guid Value { get; }

    public DeliveryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("DeliveryId no puede ser vacío.", nameof(value));
        Value = value;
    }
}
