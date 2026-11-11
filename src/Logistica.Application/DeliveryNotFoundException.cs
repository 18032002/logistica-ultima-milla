using Logistica.Domain;

namespace Logistica.Application;

public class DeliveryNotFoundException : Exception
{
    public DeliveryNotFoundException(DeliveryId id) 
        : base($"No se encontró la entrega con ID {id}.") { }
}
