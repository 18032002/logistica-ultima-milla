\# Proyecto: Sistema de Gestión Logística de Última Milla



\## Resumen ejecutivo



\- \*\*Nombre del producto:\*\* LogiControl

\- \*\*Problema principal:\*\* La empresa ha crecido, pero su sistema de logística no. Los pedidos llegan por distintos canales, los almacenes trabajan con información desactualizada y los repartidores reciben rutas sin optimizar. Esto genera clientes insatisfechos y pérdida de eficiencia interna.

\- \*\*Usuarios:\*\* Clientes, operadores de tienda, administradores de almacén, repartidores y soporte al cliente.

\- \*\*Objetivo:\*\* Centralizar la información de pedidos, garantizar trazabilidad en tiempo real y optimizar la coordinación entre almacenes y rutas de reparto.

\- \*\*Alcance:\*\* Registro de pedidos, validación de inventario, asignación de rutas, rastreo de entregas y gestión de incidencias. Quedan fuera la contabilidad, marketing y producción.



\## Entregables de la actividad 1



\- \[Análisis del problema y contexto](docs/actividad-1/analisis-problema.md)

\- \[Supuestos y decisiones iniciales](docs/actividad-1/supuestos-decisiones.md)

\- \[Diagrama de contexto](docs/actividad-1/diagrama-contexto.png)

\- \[Registro de riesgos](docs/actividad-1/riesgos.md)

\- \[Video de sustentación](Video se agrego en el Word que se adjunto también en el repositorio debido a que no estaba dejando abrir)

\---

\*Actividad 1 - Diagnóstico y contexto arquitectónico\*



\## Actividad 2 - Requisitos y decisión estructural



\### Resumen



Se tradujeron las necesidades del negocio en requisitos funcionales y no funcionales priorizados, se definieron escenarios de calidad con métricas concretas y se compararon dos alternativas estructurales (Monolito Modular vs. Arquitectura Distribuida). La decisión elegida fue un \*\*Monolito Modular con límites claros\*\*, documentada en el ADR-001.



\### Entregables



\- \[Catálogo de requisitos](docs/actividad-2/catalogo-requisitos.md)

\- \[Matriz de priorización](docs/actividad-2/matriz-priorizacion.md)

\- \[Escenarios de calidad](docs/actividad-2/escenarios-calidad.md)

\- \[Comparación de alternativas](docs/actividad-2/comparacion-alternativas.md)

\- \[ADR - Decisión arquitectónica](docs/actividad-2/adr-001-decision-estructural.md)

\- \[Video de sustentación](https://youtu.be/TU\_ENLACE\_AQUI) (enlace)







\## Actividad 3 - Diseño y dominio



\### Resumen



Se diseñó el flujo \*\*Asignar un repartidor a una entrega\*\* dentro del módulo Entregas del monolito modular. El dominio (`Delivery` como raíz de agregado) protege las invariantes; la aplicación expone `AssignCourierUseCase` y define puertos; la infraestructura los implementa con adaptadores. Las dependencias apuntan siempre hacia el dominio.



\### Entregables



\- \[Flujo de negocio](docs/actividad-3/01-flujo-negocio.md)

\- \[Glosario de lenguaje ubicuo](docs/actividad-3/02-glosario-lenguaje-ubicuo.md)

\- \[Modelo de dominio y estados](docs/actividad-3/03-modelo-dominio.md)

\- \[Invariantes y escenarios Dado-Cuando-Entonces](docs/actividad-3/04-invariantes-escenarios.md)

\- \[Contextos delimitados y contratos](docs/actividad-3/05-contextos-contratos.md)

\- \[Casos de uso, puertos y adaptadores](docs/actividad-3/06-casos-uso-puertos-adaptadores.md)

\- \[Diagrama C4](docs/actividad-3/07-diagrama-c4.md)

\- \[ADR-002](docs/actividad-3/adr-002-diseno-dominio.md)

\- \[Architecture.md](Architecture.md)

- Código: [src/](src) (Domain, Application, Infrastructure) y [tests/](tests)

\- \[Video de sustentación](https://youtu.be/TU\_ENLACE\_AQUI) (enlace)



## Actividad 4 - Construcción, validación y defensa final

### Resumen de la implementación

Se implementó el vertical slice del caso de uso **Asignar repartidor a una entrega** (`AssignCourierUseCase`) aplicando Clean Architecture, con las siguientes capas:
- **Domain:** Entidades (`Delivery`), Objetos de Valor (`DeliveryId`, `CourierId`, `DeliveryAddress`) e Invariantes (INV-01 a INV-08).
- **Application:** Caso de uso (`AssignCourierUseCase`), comandos y puertos de salida (`IDeliveryRepository`, `ICourierAvailabilityPort`, `IEventPublisher`).
- **Infrastructure:** Adaptadores en memoria (`InMemoryDeliveryRepository`, `MockCourierAvailabilityAdapter`, `ConsoleEventPublisher`).
- **API:** Endpoint REST en ASP.NET Core (`POST /api/deliveries/{id}/assign-courier`).

### Pruebas y Validación

- **Pruebas Unitarias (Dominio):** 3 pruebas verificando el comportamiento de las entidades y la protección de invariantes.
- **Pruebas de Integración (Flujo):** 3 pruebas verificando el caso de uso completo (éxito, fallo por disponibilidad, fallo por entrega inexistente).
- **Pruebas de Arquitectura (Fitness Functions):** 2 pruebas verificando que las dependencias apunten hacia adentro (Domain no depende de nadie; Application no depende de Infrastructure).
- **Resultado total:** 8 pruebas superadas, 0 errores.

### Riesgos y Observabilidad

- **Riesgos:** Documentados en [docs/actividad-4-riesgos.md](docs/actividad-4-riesgos.md), con sus mitigaciones y evidencias de verificación.
- **Observabilidad:** La capa de entrada cuenta con logs estructurados (usando `ILogger`) que registran IDs técnicos (DeliveryId, CourierId) y niveles de severidad (Information, Warning, Error), evitando exponer datos personales o direcciones.

### Instrucciones de Ejecución

Para ejecutar la API localmente:
1. Abrir una terminal en la raíz del repositorio.
2. Ejecutar `dotnet run --project src/Logistica.Api`
3. La API estará disponible en `https://localhost:xxxx` (revisar la consola).
4. (Opcional) Puedes usar el endpoint `POST /api/deliveries/seed` para crear una entrega de prueba, y luego usar `POST /api/deliveries/{id}/assign-courier` enviando un JSON como `{ "courierId": "11111111-1111-1111-1111-111111111111" }`.

### Video de sustentación (Defensa final)

- [Video de demostración y defensa final](https://youtu.be/TU_ENLACE_AQUI) (enlace)
