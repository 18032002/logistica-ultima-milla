using Logistica.Domain;

namespace Logistica.Application;

public interface ICourierAvailabilityPort
{
    Task<bool> IsAvailableAsync(CourierId courierId, CancellationToken ct = default);
}
