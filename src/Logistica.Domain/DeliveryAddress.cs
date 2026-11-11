namespace Logistica.Domain;

public sealed record DeliveryAddress
{
    public string Value { get; }

    public DeliveryAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("La dirección de entrega no puede estar vacía (INV-08).");
        
        Value = value;
    }

    public override string ToString() => Value;
}
