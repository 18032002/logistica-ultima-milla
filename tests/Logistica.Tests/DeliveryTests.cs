using Logistica.Domain;
using Xunit;

namespace Logistica.Tests;

public class DeliveryTests
{
    [Fact]
    public void AssignCourier_Should_SetCourierAndChangeStatus_When_DeliveryIsPending()
    {
        // Arrange (Preparar)
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));
        var courierId = CourierId.New();

        // Act (Actuar)
        delivery.AssignCourier(courierId);

        // Assert (Verificar)
        Assert.Equal(courierId, delivery.CourierId);
        Assert.Equal(DeliveryStatus.Assigned, delivery.Status);
    }

    [Fact]
    public void AssignCourier_Should_ThrowException_When_DeliveryAlreadyHasCourier()
    {
        // Arrange
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));
        var firstCourier = CourierId.New();
        var secondCourier = CourierId.New();
        delivery.AssignCourier(firstCourier); // Primera asignación exitosa

        // Act & Assert (Verificar que lance la excepción)
        var exception = Assert.Throws<DomainException>(() => delivery.AssignCourier(secondCourier));
        
        // El dominio lanza la excepción de estado inválido primero (INV-03)
        Assert.Contains("estado Assigned", exception.Message);
    }

    [Fact]
    public void AssignCourier_Should_ThrowException_When_DeliveryIsDelivered()
    {
        // Arrange
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));
        // Usamos reflexión para forzar el estado a "Entregado" y probar la INV-03
        typeof(Delivery).GetProperty("Status")!.SetValue(delivery, DeliveryStatus.Delivered);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => delivery.AssignCourier(CourierId.New()));
        Assert.Contains("estado Delivered", exception.Message);
    }
}
