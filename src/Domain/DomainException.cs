namespace Domain;

public enum DomainError
{
    AlreadyAssigned,
    InvalidState,
    ReasonRequired
}

public sealed class DomainException : Exception
{
    public DomainError Error { get; }

    public DomainException(DomainError error, string message) : base(message)
    {
        Error = error;
    }
}
