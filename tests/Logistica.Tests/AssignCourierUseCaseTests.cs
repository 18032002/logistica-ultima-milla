using Logistica.Application;
using Logistica.Domain;
using Logistica.Infrastructure;
using Xunit;

namespace Logistica.Tests;

public class AssignCourierUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_Should_AssignCourierAndPublishEvent_When_FlowIsSuccessful()
    {
        // Arrange (Usamos los adaptadores reales de Infraestructura pero en memoria)
        var repo = new InMemoryDeliveryRepository();
        var courierService = new MockCourierAvailabilityAdapter();
        var eventPublisher = new ConsoleEventPublisher();
        var useCase = new AssignCourierUseCase(repo, courierService, eventPublisher);

        var deliveryId = DeliveryId.New();
        var delivery = new Delivery(deliveryId, new DeliveryAddress("Avenida Siempreviva 742"));
        await repo.SaveAsync(delivery);

        // Guid que SÍ está disponible (no termina en 0)
        var courierId = new CourierId(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var command = new AssignCourierCommand(deliveryId, courierId);

        // Act
        await useCase.ExecuteAsync(command);

        // Assert
        var updatedDelivery = await repo.GetByIdAsync(deliveryId);
        Assert.NotNull(updatedDelivery);
        Assert.Equal(courierId, updatedDelivery.CourierId);
        Assert.Equal(DeliveryStatus.Assigned, updatedDelivery.Status);
    }

    [Fact]
    public async Task ExecuteAsync_Should_ThrowException_When_CourierIsNotAvailable()
    {
        // Arrange
        var repo = new InMemoryDeliveryRepository();
        var courierService = new MockCourierAvailabilityAdapter();
        var eventPublisher = new ConsoleEventPublisher();
        var useCase = new AssignCourierUseCase(repo, courierService, eventPublisher);

        var deliveryId = DeliveryId.New();
        var delivery = new Delivery(deliveryId, new DeliveryAddress("Avenida Siempreviva 742"));
        await repo.SaveAsync(delivery);

        // Guid que NO está disponible según nuestra simulación (termina en 0)
        var courierId = new CourierId(Guid.Parse("22222222-2222-2222-2222-222222222220"));
        var command = new AssignCourierCommand(deliveryId, courierId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CourierNotAvailableException>(
            () => useCase.ExecuteAsync(command));
        
        Assert.Contains("no está disponible", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_Should_ThrowException_When_DeliveryDoesNotExist()
    {
        // Arrange
        var repo = new InMemoryDeliveryRepository();
        var useCase = new AssignCourierUseCase(repo, new MockCourierAvailabilityAdapter(), new ConsoleEventPublisher());
        
        var command = new AssignCourierCommand(DeliveryId.New(), CourierId.New());

        // Act & Assert
        await Assert.ThrowsAsync<DeliveryNotFoundException>(() => useCase.ExecuteAsync(command));
    }
}
