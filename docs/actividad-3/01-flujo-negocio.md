\# Flujo de negocio: Asignar un repartidor a una entrega



\## Propósito



Garantizar que cada entrega lista para salir tenga exactamente un repartidor

disponible, con una ruta que no esté saturada, y que la asignación quede registrada.



\## Actores



| Actor | Rol en el flujo |

|-------|-----------------|

| Despachador (o el sistema de asignación automática) | Solicita asignar un repartidor a una entrega |

| Repartidor | Recibe la entrega asignada |

| Cliente | Es notificado de que su pedido ya tiene repartidor |

| Servicio de disponibilidad / rutas (externo) | Informa si el repartidor está disponible y si su ruta tiene capacidad |



\## Precondiciones



\- La entrega existe y está en estado `ListaParaDespacho`.

\- El pedido ya fue pagado y su stock fue validado (flujo previo, fuera de esta actividad).



\## Resultado esperado



\- La entrega queda con un repartidor activo y pasa a estado `Asignada`.

\- Se registra quién asignó, a quién y cuándo (trazabilidad, RNF-08).

\- Se notifica el resultado al repartidor y al cliente.



\## Resultados alternativos



\- La entrega no existe.

\- La entrega ya tiene un repartidor activo.

\- El repartidor no está disponible.

\- La ruta del repartidor está saturada.



\## Relación con la Actividad 2



\- Requisitos: RF-03 (Asignar ruta), RF-04 (Rastrear entrega), RNF-05 (Mantenibilidad), RNF-08 (Trazabilidad).

\- Decisión: vive dentro del \*\*Módulo de Entregas\*\* del monolito modular (ADR-001).

