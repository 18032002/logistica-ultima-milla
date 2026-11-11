using Logistica.Application;

namespace Logistica.Infrastructure;

public class ConsoleEventPublisher : IEventPublisher
{
    public Task PublishAsync<T>(T domainEvent, CancellationToken ct = default) where T : class
    {
        // En un caso real, aquí se enviaría a RabbitMQ, Kafka o un bus de eventos.
        Console.WriteLine($"[EVENTO PUBLICADO] {typeof(T).Name}: {domainEvent}");
        return Task.CompletedTask;
    }
}
