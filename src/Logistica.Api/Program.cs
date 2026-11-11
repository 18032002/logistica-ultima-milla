using Logistica.Application;
using Logistica.Domain;
using Logistica.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar soporte para Controladores
builder.Services.AddControllers();

// 2. Documentación de API nativa de .NET 9/10
builder.Services.AddOpenApi(); 

// 3. Inyección de Dependencias (Adaptadores e Infraestructura)
builder.Services.AddSingleton<IDeliveryRepository, InMemoryDeliveryRepository>();
builder.Services.AddSingleton<ICourierAvailabilityPort, MockCourierAvailabilityAdapter>();
builder.Services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();
builder.Services.AddScoped<IAssignCourierUseCase, AssignCourierUseCase>();

var app = builder.Build();

// 4. Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

