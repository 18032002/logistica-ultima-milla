\# Casos de uso, puertos y adaptadores



\## Capas y dirección de dependencias



| Capa | Carpeta | Contiene | Depende de |

|------|---------|----------|------------|

| Dominio | `/src/Domain` | Entidades, objetos de valor, invariantes | Nada |

| Aplicación | `/src/Application` | Casos de uso, DTOs, puertos | Solo Dominio |

| Infraestructura | `/src/Infrastructure` | Adaptadores (BD, APIs, notificaciones) | Aplicación y Dominio |



\*\*Regla:\*\* las flechas de dependencia apuntan siempre hacia el dominio. Infraestructura implementa las interfaces que define Aplicación (inversión de dependencias, la "D" de SOLID).



```mermaid

flowchart BT

&#x20;   Infra\["Infrastructure<br>(adaptadores)"]

&#x20;   App\["Application<br>(casos de uso + puertos)"]

&#x20;   Dom\["Domain<br>(entidades, VO, invariantes)"]



&#x20;   Infra -->|"implementa puertos de"| App

&#x20;   App -->|"usa"| Dom

&#x20;   Infra -->|"usa"| Dom

```



\## Casos de uso



| Caso de uso | Intención | Escenarios que lo cubren |

|-------------|-----------|--------------------------|

| `AssignCourierUseCase` | Asignar un repartidor a una entrega lista para despacho | 1 al 6 y 8 |

| `CancelDeliveryUseCase` | Cancelar una entrega indicando motivo | 7 |

| `GetDeliveryStatusUseCase` | Consultar el estado de una entrega (RF-04) | Fuera de esta actividad |



Se modela a fondo `AssignCourierUseCase`; los otros quedan descritos.



\## Contrato de aplicación: AssignCourierUseCase



\*\*Entrada (`AssignCourierCommand`):\*\*



| Campo | Tipo | Descripción |

|-------|------|-------------|

| `DeliveryId` | Guid | Entrega a asignar |

| `CourierId` | Guid | Repartidor elegido |

| `RequestedBy` | string | Quién solicita (trazabilidad) |



\*\*Salida (`AssignCourierResult`):\*\*



| Campo | Tipo | Descripción |

|-------|------|-------------|

| `Success` | bool | Resultado |

| `ErrorCode` | enum | `DeliveryNotFound`, `AlreadyAssigned`, `InvalidState`, `CourierUnavailable`, `RouteSaturated`, `AvailabilityCheckFailed` |

| `DeliveryStatus` | string | Estado final de la entrega |



El caso de uso no devuelve entidades del dominio ni lanza detalles de HTTP o SQL: devuelve un resultado de aplicación.



\## Flujo del caso de uso



1\. Cargar la entrega con `IDeliveryRepository`. Si no existe: `DeliveryNotFound`.

2\. Verificar que no tenga repartidor activo: `AlreadyAssigned`.

3\. Consultar `ICourierAvailabilityPort`. Si falla: `AvailabilityCheckFailed`.

4\. Si no está disponible o no tiene cupo: `CourierUnavailable` o `RouteSaturated`.

5\. Ejecutar `delivery.AssignCourier(courierId)` (el dominio protege INV-01, INV-02, INV-05).

6\. Guardar con `IDeliveryRepository.SaveAsync`.

7\. Registrar con `IAuditLog` y publicar con `IEventPublisher`.



```mermaid

sequenceDiagram

&#x20;   participant D as Despachador

&#x20;   participant UC as AssignCourierUseCase

&#x20;   participant Repo as IDeliveryRepository

&#x20;   participant Disp as ICourierAvailabilityPort

&#x20;   participant Dom as Delivery (dominio)

&#x20;   participant Aud as IAuditLog

&#x20;   participant Pub as IEventPublisher



&#x20;   D->>UC: ExecuteAsync(command)

&#x20;   UC->>Repo: GetAsync(deliveryId)

&#x20;   Repo-->>UC: Delivery

&#x20;   UC->>Disp: CheckAsync(courierId, window)

&#x20;   Disp-->>UC: CourierAvailability

&#x20;   UC->>Dom: AssignCourier(courierId)

&#x20;   UC->>Repo: SaveAsync(delivery)

&#x20;   UC->>Aud: RecordAsync(asignación)

&#x20;   UC->>Pub: PublishAsync(EntregaAsignada)

&#x20;   UC-->>D: AssignCourierResult

```



\## Puertos (interfaces definidas en Application)



| Puerto | Tipo | Responsabilidad |

|--------|------|-----------------|

| `IDeliveryRepository` | Salida | Cargar y guardar entregas |

| `ICourierAvailabilityPort` | Salida | Consultar disponibilidad y cupo de ruta |

| `IAuditLog` | Salida | Registrar trazabilidad (RNF-08) |

| `IEventPublisher` | Salida | Publicar `EntregaAsignada` |

| `IClock` | Salida | Obtener la hora actual (evita `DateTime.Now` y facilita pruebas) |



\## Adaptadores (implementados en Infrastructure)



| Puerto | Adaptador (ahora) | Adaptador (futuro) |

|--------|-------------------|--------------------|

| `IDeliveryRepository` | `InMemoryDeliveryRepository` (pruebas) | `SqlDeliveryRepository` |

| `ICourierAvailabilityPort` | `FakeCourierAvailabilityAdapter` | `FleetApiAdapter` (con ACL) |

| `IAuditLog` | `ConsoleAuditLog` | `DatabaseAuditLog` |

| `IEventPublisher` | `InMemoryEventPublisher` | `NotificationEventPublisher` (correo/SMS) |

| `IClock` | `SystemClock` | (igual) |



\## Cómo se evita acoplar el negocio a la infraestructura



\- El caso de uso recibe sus dependencias \*\*por constructor\*\* y solo conoce interfaces.

\- No hay `SqlConnection`, `HttpClient` ni clases de proveedor en Domain ni en Application.

\- Cambiar de base de datos o de proveedor de flota solo toca un adaptador.

\- Las pruebas del dominio y del caso de uso corren con adaptadores en memoria, sin BD ni red.



\## Trade-offs



| Beneficio | Costo |

|-----------|-------|

| Dominio testeable sin infraestructura | Más interfaces y archivos que un CRUD directo |

| Cambiar proveedores sin tocar el negocio | Hay que mantener la traducción (ACL) |

| Reglas centralizadas en el dominio | Requiere disciplina para no filtrar lógica a la infraestructura |

