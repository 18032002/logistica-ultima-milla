using Logistica.Domain;

namespace Logistica.Application;

public record AssignCourierCommand(DeliveryId DeliveryId, CourierId CourierId);
