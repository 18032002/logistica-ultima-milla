namespace Domain;

public sealed record CourierId
{
    public Guid Value { get; }

    public CourierId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CourierId no puede ser vacío.", nameof(value));
        Value = value;
    }
}
