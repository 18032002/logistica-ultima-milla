namespace Logistica.Api;

public record AssignCourierRequest(Guid DeliveryId, Guid CourierId);
