# Proyecto: Sistema de Gestión Logística de Última Milla (LogiControl)

## Resumen ejecutivo

- **Problema principal:** La empresa ha crecido, pero su sistema de logística no. Los pedidos llegan por distintos canales, los almacenes trabajan con información desactualizada y los repartidores reciben rutas sin optimizar. Esto genera clientes insatisfechos y pérdida de eficiencia interna.
- **Usuarios:** Clientes, operadores de tienda, administradores de almacén, repartidores y soporte al cliente.
- **Objetivo:** Centralizar la información de pedidos, garantizar trazabilidad en tiempo real y optimizar la coordinación entre almacenes y rutas de reparto.
- **Alcance:** Registro de pedidos, validación de inventario, asignación de rutas, rastreo de entregas y gestión de incidencias. Quedan fuera la contabilidad, marketing y producción.

---

## Actividad 1 - Diagnóstico y contexto arquitectónico

### Resumen
Se definió el problema central, los actores del sistema, el alcance del proyecto, los riesgos operativos y las decisiones iniciales de arquitectura.

### Entregables
- [Análisis del problema y contexto](docs/actividad-1/analisis-problema.md)
- [Supuestos y decisiones iniciales](docs/actividad-1/supuestos-decisiones.md)
- [Diagrama de contexto](docs/actividad-1/diagrama-contexto.png)
- [Registro de riesgos](docs/actividad-1/riesgos.md)
- [Video de sustentación Actividad 1](Video se agregó en el Word adjunto en el repositorio debido a que no estaba dejando abrir)

---

## Actividad 2 - Requisitos y decisión estructural

### Resumen
Se tradujeron las necesidades del negocio en requisitos funcionales y no funcionales priorizados, se definieron escenarios de calidad con métricas concretas y se compararon dos alternativas estructurales (Monolito Modular vs. Arquitectura Distribuida). La decisión elegida fue un **Monolito Modular con límites claros**, documentada en el ADR-001.

### Entregables
- [Catálogo de requisitos](docs/actividad-2/catalogo-requisitos.md)
- [Matriz de priorización](docs/actividad-2/matriz-priorizacion.md)
- [Escenarios de calidad](docs/actividad-2/escenarios-calidad.md)
- [Comparación de alternativas](docs/actividad-2/comparacion-alternativas.md)
- [ADR-001 - Decisión arquitectónica](docs/actividad-2/adr-001-decision-estructural.md)
- [Video de sustentación Actividad 2](https://youtu.be/TU_ENLACE_AQUI)

---

## Actividad 3 - Diseño y dominio

### Resumen
Se modeló el flujo **Asignar repartidor a una entrega** (`AssignCourierUseCase`) aplicando Domain-Driven Design (DDD) y Clean Architecture. El dominio protege las invariantes; la aplicación expone los casos de uso y define los puertos; la infraestructura implementa los adaptadores. Las dependencias apuntan siempre hacia el dominio.

### Entregables
- [Flujo de negocio](docs/actividad-3/01-flujo-negocio.md)
- [Glosario de lenguaje ubicuo](docs/actividad-3/02-glosario-lenguaje-ubicuo.md)
- [Modelo de dominio y estados](docs/actividad-3/03-modelo-dominio.md)
- [Invariantes y escenarios Dado-Cuando-Entonces](docs/actividad-3/04-invariantes-escenarios.md)
- [Contextos delimitados y contratos](docs/actividad-3/05-contextos-contratos.md)
- [Casos de uso, puertos y adaptadores](docs/actividad-3/06-casos-uso-puertos-adaptadores.md)
- [Diagrama C4](docs/actividad-3/07-diagrama-c4.md)
- [ADR-002 - Diseño del dominio](docs/actividad-3/adr-002-diseno-dominio.md)
- [Architecture.md](Architecture.md)
- [Video de sustentación Actividad 3](https://youtu.be/TU_ENLACE_AQUI)

---

## Actividad 4 - Construcción, validación y defensa final

### Resumen de la implementación
Se implementó el vertical slice del caso de uso **Asignar repartidor a una entrega** aplicando Clean Architecture, con las siguientes capas:
- **Domain:** Entidades (`Delivery`), Objetos de Valor (`DeliveryId`, `CourierId`, `DeliveryAddress`) e Invariantes.
- **Application:** Caso de uso (`AssignCourierUseCase`), comandos y puertos de salida.
- **Infrastructure:** Adaptadores en memoria (`InMemoryDeliveryRepository`, `MockCourierAvailabilityAdapter`, `ConsoleEventPublisher`).
- **API:** Endpoint REST en ASP.NET Core (`POST /api/deliveries/{id}/assign-courier`).

### Pruebas y Validación
- **Pruebas Unitarias (Dominio):** 3 pruebas verificando el comportamiento de las entidades y la protección de invariantes.
- **Pruebas de Integración (Flujo):** 3 pruebas verificando el caso de uso completo (éxito, fallo por disponibilidad, fallo por entrega inexistente).
- **Pruebas de Arquitectura (Fitness Functions):** 2 pruebas verificando que las dependencias apunten hacia adentro.
- **Resultado total:** 8 pruebas superadas, 0 errores.

### Riesgos y Observabilidad
- **Riesgos:** Documentados en [Registro de riesgos Actividad 4](docs/actividad-4-riesgos.md), con sus mitigaciones y evidencias de verificación.
- **Observabilidad:** La capa de entrada cuenta con logs estructurados (usando `ILogger`) que registran IDs técnicos y niveles de severidad, evitando exponer datos personales.

### Instrucciones de Ejecución Local
Para ejecutar la API localmente:
1. Abrir una terminal en la raíz del repositorio.
2. Ejecutar `dotnet run --project src/Logistica.Api`.
3. La API estará disponible en la URL que muestre la consola (ej. `https://localhost:xxxx`).
4. (Opcional) Usar el endpoint `POST /api/deliveries/seed` para crear una entrega de prueba.
5. Usar el endpoint `POST /api/deliveries/{id}/assign-courier` enviando un JSON como:
```json
{ 
  "courierId": "11111111-1111-1111-1111-111111111111" 
}
