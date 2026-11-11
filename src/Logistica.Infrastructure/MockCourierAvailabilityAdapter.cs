using Logistica.Application;
using Logistica.Domain;

namespace Logistica.Infrastructure;

public class MockCourierAvailabilityAdapter : ICourierAvailabilityPort
{
    public Task<bool> IsAvailableAsync(CourierId courierId, CancellationToken ct = default)
    {
        // Simulamos que si el Guid termina en '0', el repartidor NO está disponible.
        // Esto nos servirá para probar el caso de error en las pruebas.
        var idStr = courierId.Value.ToString();
        bool isAvailable = !idStr.EndsWith("0");

        return Task.FromResult(isAvailable);
    }
}
