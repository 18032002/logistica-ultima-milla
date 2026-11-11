using Logistica.Domain;

namespace Logistica.Application;

public class CourierNotAvailableException : Exception
{
    public CourierNotAvailableException(CourierId id) 
        : base($"El repartidor {id} no está disponible o superó su capacidad (INV-02).") { }
}
