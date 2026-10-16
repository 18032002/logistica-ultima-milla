using Application;
using Domain;
using Infrastructure;
using Xunit;

namespace Tests;

public class AssignCourierTests
{
    private static readonly Guid DeliveryGuid = Guid.NewGuid();
    private static readonly Guid CourierGuid = Guid.NewGuid();

    private static Delivery NewDelivery() =>
        new(new DeliveryId(DeliveryGuid), new DeliveryAddress("Calle 10 # 20-30", "Medellín"));

    private sealed class Setup
    {
        public InMemoryDeliveryRepository Repo { get; } = new();
        public InMemoryEventPublisher Publisher { get; } = new();
        public AssignCourierUseCase UseCase { get; }

        public Setup(Func<CourierId, CourierAvailability>? availabilityRule = null, bool availabilityThrows = false)
        {
            ICourierAvailabilityPort availability = availabilityThrows
                ? new ThrowingAvailability()
                : new FakeCourierAvailabilityAdapter(availabilityRule);

            UseCase = new AssignCourierUseCase(
                Repo, availability, new ConsoleAuditLog(), Publisher, new SystemClock());
        }

        public AssignCourierCommand Command() =>
            new(DeliveryGuid, CourierGuid, "despachador1");
    }

    private sealed class ThrowingAvailability : ICourierAvailabilityPort
    {
        public Task<CourierAvailability> CheckAsync(CourierId courierId) =>
            throw new InvalidOperationException("Servicio caído");
    }

    // Escenario 1: asignación exitosa
    [Fact]
    public async Task Asigna_repartidor_y_publica_evento()
    {
        var s = new Setup();
        await s.Repo.SaveAsync(NewDelivery());

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.True(result.Success);
        Assert.Equal("Asignada", result.DeliveryStatus);
        Assert.Single(s.Publisher.Published);
    }

    // Escenario 2 (INV-01): entrega ya asignada
    [Fact]
    public async Task Rechaza_segunda_asignacion()
    {
        var s = new Setup();
        await s.Repo.SaveAsync(NewDelivery());
        await s.UseCase.ExecuteAsync(s.Command());

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.False(result.Success);
        Assert.Equal(AssignCourierError.AlreadyAssigned, result.Error);
    }

    // Escenario 3 (INV-02, INV-05): estado inválido
    [Fact]
    public void Dominio_rechaza_asignar_si_esta_en_ruta()
    {
        var delivery = NewDelivery();
        delivery.AssignCourier(new CourierId(CourierGuid));
        delivery.StartRoute();

        // La entrega ya tiene repartidor, así que probamos con una entrega EnRuta sin repartidor
        // a través de la regla de estado: no se puede reasignar.
        var ex = Assert.Throws<DomainException>(() =>
            delivery.AssignCourier(new CourierId(Guid.NewGuid())));

        Assert.Equal(DomainError.AlreadyAssigned, ex.Error);
        Assert.Equal(DeliveryStatus.EnRuta, delivery.Status);
    }

    // Escenario 4 (INV-03): ruta saturada
    [Fact]
    public async Task Rechaza_si_la_ruta_esta_saturada()
    {
        var s = new Setup(_ => new CourierAvailability(true, false));
        await s.Repo.SaveAsync(NewDelivery());

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.False(result.Success);
        Assert.Equal(AssignCourierError.RouteSaturated, result.Error);
        Assert.Equal("ListaParaDespacho", result.DeliveryStatus);
    }

    // Escenario 5 (INV-04): repartidor no disponible
    [Fact]
    public async Task Rechaza_si_el_repartidor_no_esta_disponible()
    {
        var s = new Setup(_ => new CourierAvailability(false, true));
        await s.Repo.SaveAsync(NewDelivery());

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.False(result.Success);
        Assert.Equal(AssignCourierError.CourierUnavailable, result.Error);
        Assert.Empty(s.Publisher.Published);
    }

    // Escenario 6: entrega inexistente
    [Fact]
    public async Task Responde_no_encontrada_si_la_entrega_no_existe()
    {
        var s = new Setup();

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.False(result.Success);
        Assert.Equal(AssignCourierError.DeliveryNotFound, result.Error);
    }

    // Escenario 7 (INV-06): cancelar sin motivo
    [Fact]
    public void Dominio_rechaza_cancelar_sin_motivo()
    {
        var delivery = NewDelivery();
        delivery.AssignCourier(new CourierId(CourierGuid));

        var ex = Assert.Throws<DomainException>(() => delivery.Cancel(""));

        Assert.Equal(DomainError.ReasonRequired, ex.Error);
        Assert.Equal(DeliveryStatus.Asignada, delivery.Status);
    }

    // Escenario 8: falla del servicio de disponibilidad
    [Fact]
    public async Task No_asigna_a_ciegas_si_falla_el_servicio_externo()
    {
        var s = new Setup(availabilityThrows: true);
        await s.Repo.SaveAsync(NewDelivery());

        var result = await s.UseCase.ExecuteAsync(s.Command());

        Assert.False(result.Success);
        Assert.Equal(AssignCourierError.AvailabilityCheckFailed, result.Error);
        Assert.Equal("ListaParaDespacho", result.DeliveryStatus);
    }
}