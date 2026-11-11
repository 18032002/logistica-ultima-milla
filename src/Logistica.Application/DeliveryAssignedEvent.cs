using Logistica.Domain;

namespace Logistica.Application;

public record DeliveryAssignedEvent(DeliveryId DeliveryId, CourierId CourierId);
