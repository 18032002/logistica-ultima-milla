namespace Logistica.Domain;

public sealed record DeliveryId(Guid Value)
{
    public static DeliveryId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}