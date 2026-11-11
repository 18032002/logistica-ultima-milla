using NetArchTest.Rules;
using Xunit;

namespace Logistica.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Logistica.Domain";
    private const string ApplicationNamespace = "Logistica.Application";
    private const string InfrastructureNamespace = "Logistica.Infrastructure";

    [Fact]
    public void Domain_Should_Not_DependOn_Application_Or_Infrastructure()
    {
        // Arrange
        var assembly = typeof(Logistica.Domain.Delivery).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "El Dominio no debe depender de capas superiores (Clean Architecture).");
    }

    [Fact]
    public void Application_Should_Not_DependOn_Infrastructure()
    {
        // Arrange
        var assembly = typeof(Logistica.Application.AssignCourierUseCase).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "La Aplicación no debe depender de la Infraestructura (Puertos y Adaptadores).");
    }
}
