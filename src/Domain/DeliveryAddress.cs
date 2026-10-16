namespace Domain;

public sealed record DeliveryAddress
{
    public string Street { get; }
    public string City { get; }
    public string? Reference { get; }

    public DeliveryAddress(string street, string city, string? reference = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("La calle es obligatoria.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("La ciudad es obligatoria.", nameof(city));
        Street = street;
        City = city;
        Reference = reference;
    }
}
