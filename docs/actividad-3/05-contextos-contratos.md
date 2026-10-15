\# Contextos delimitados y contratos



\## Contextos involucrados en el flujo "Asignar repartidor"



| Contexto | Tipo | Responsabilidad | Modelo propio (lo que "ve") |

|----------|------|-----------------|------------------------------|

| \*\*Entregas\*\* | Núcleo (core) | Ciclo de vida de la entrega y asignación de repartidor | `Delivery`, `Courier`, `Route` |

| \*\*Pedidos\*\* | Soporte | Ciclo de vida del pedido, pago y validación de stock | `Order`, `OrderLine` |

| \*\*Flota / Disponibilidad\*\* | Soporte (externo o interno) | Turnos, disponibilidad y cupo de rutas de repartidores | `CourierAvailability` |

| \*\*Notificaciones\*\* | Genérico | Enviar mensajes al repartidor y al cliente | `Notification` |

| \*\*Auditoría\*\* | Genérico | Registrar trazabilidad de acciones (RNF-08) | `AuditRecord` |



> \*\*Entregas\*\* es el contexto núcleo porque ahí está la ventaja competitiva del negocio: coordinar bien la logística. Los demás se tratan como soporte o genéricos.



\## Mapa de contextos



```mermaid

flowchart LR

&#x20;   Pedidos\["Pedidos<br>(upstream)"]

&#x20;   Entregas\["Entregas<br>(núcleo)"]

&#x20;   Flota\["Flota / Disponibilidad<br>(externo o interno)"]

&#x20;   Notif\["Notificaciones"]

&#x20;   Audit\["Auditoría"]



&#x20;   Pedidos -->|"evento: PedidoListoParaDespacho"| Entregas

&#x20;   Entregas -->|"consulta: ¿disponible y con cupo?"| Flota

&#x20;   Entregas -->|"evento: EntregaAsignada"| Notif

&#x20;   Entregas -->|"registro: AsignacionRegistrada"| Audit

```



\## Relaciones entre contextos



| Relación | Patrón (context map) | Explicación |

|----------|----------------------|-------------|

| Pedidos → Entregas | \*\*Cliente-proveedor (upstream/downstream)\*\* | Pedidos publica un hecho; Entregas lo consume y crea su propia `Delivery`. Entregas no modifica pedidos. |

| Entregas → Flota | \*\*Capa anticorrupción (ACL)\*\* | Si el proveedor de flota cambia su formato, solo se adapta el traductor. El dominio de Entregas no se entera. |

| Entregas → Notificaciones | \*\*Publicador / suscriptor\*\* | Entregas emite un evento; Notificaciones decide cómo avisar (correo, SMS). |

| Entregas → Auditoría | \*\*Conformista\*\* | Auditoría define un formato de registro simple y Entregas se ajusta a él. |



\## Contratos (lo que cruza cada frontera)



\### Contrato 1: Pedidos → Entregas (evento de entrada)



| Campo | Tipo | Descripción |

|-------|------|-------------|

| `OrderId` | Guid | Pedido de origen |

| `Address` | texto estructurado | Calle, ciudad, referencia |

| `DeliveryWindowFrom` / `To` | fecha-hora | Ventana prometida |



Entregas lo traduce a su propio `Delivery`. No copia datos de pago ni de líneas de pedido: no los necesita.



\### Contrato 2: Entregas → Flota (consulta de disponibilidad)



\*\*Petición:\*\* `CourierId`, `DeliveryWindow`



\*\*Respuesta:\*\*



| Campo | Tipo | Descripción |

|-------|------|-------------|

| `IsAvailable` | bool | Repartidor en turno |

| `HasRouteCapacity` | bool | Ruta con cupo |

| `Reason` | texto opcional | Motivo si no está disponible |



Se traduce a `CourierAvailability` (objeto de valor del dominio).



\### Contrato 3: Entregas → Notificaciones (evento de salida)



`EntregaAsignada`: `DeliveryId`, `CourierId`, `AssignedAtUtc`



\### Contrato 4: Entregas → Auditoría (registro)



`AsignacionRegistrada`: `DeliveryId`, `CourierId`, `AssignedBy`, `AssignedAtUtc`



\## Lenguaje por contexto (mismo concepto, distinto significado)



| Concepto | En Pedidos | En Entregas |

|----------|-----------|-------------|

| Dirección | Dirección de facturación y envío del cliente | `DeliveryAddress`: lugar concreto de entrega |

| Estado | Pendiente, Pagado, Cancelado | `DeliveryStatus`: ListaParaDespacho, Asignada, EnRuta... |

| Cancelar | Anular la compra | Anular el traslado (el pedido puede seguir vigente) |



\## Decisiones de este paso



\- Cada contexto tiene \*\*su propio modelo\*\*; no se comparte una clase `Order` entre módulos.

\- La comunicación entre módulos es por \*\*eventos y contratos\*\*, no por acceso directo a tablas ajenas. Esto deja la puerta abierta a extraer módulos como servicios, como previó el ADR-001.

