\# Invariantes y escenarios de aceptación (contexto: Entregas)



\## Invariantes



| ID | Invariante | Quién la protege |

|----|-----------|------------------|

| INV-01 | Una entrega no puede tener más de un repartidor activo. | `Delivery.AssignCourier` |

| INV-02 | Solo se puede asignar repartidor a una entrega en estado `ListaParaDespacho`. | `Delivery.AssignCourier` |

| INV-03 | Una entrega no puede asignarse a un repartidor si su ruta ya está saturada. | `Route.AddDelivery` + caso de uso |

| INV-04 | Una entrega no puede asignarse a un repartidor que no esté disponible. | Caso de uso (consulta al puerto de disponibilidad) |

| INV-05 | Una entrega no puede estar en dos estados contradictorios a la vez. | `Delivery` (transiciones controladas) |

| INV-06 | Una entrega fallida o cancelada requiere un motivo. | `Delivery.MarkFailed` / `Delivery.Cancel` |

| INV-07 | Toda asignación queda registrada con quién, a quién y cuándo (RNF-08). | Caso de uso (puerto de auditoría) |



> Nota de diseño: INV-01, INV-02, INV-05 y INV-06 dependen solo del estado interno de `Delivery`, por eso viven en el dominio. INV-03 y INV-04 requieren información externa (disponibilidad), por eso el caso de uso la consulta mediante un puerto y luego el dominio ejecuta la regla.



\## Escenarios de aceptación



\### Escenario 1: Asignación exitosa (camino feliz)

\- \*\*Dado\*\* una entrega en estado `ListaParaDespacho` sin repartidor

\- \*\*Y\*\* un repartidor disponible con cupo en su ruta

\- \*\*Cuando\*\* el despachador asigna ese repartidor a la entrega

\- \*\*Entonces\*\* la entrega queda en estado `Asignada` con ese repartidor

\- \*\*Y\*\* se registra la asignación (quién, a quién, cuándo)

\- \*\*Y\*\* se notifica al repartidor y al cliente



\### Escenario 2: Entrega ya asignada (INV-01)

\- \*\*Dado\*\* una entrega en estado `Asignada` con un repartidor activo

\- \*\*Cuando\*\* se intenta asignar otro repartidor

\- \*\*Entonces\*\* el sistema rechaza la operación con el error "La entrega ya tiene repartidor"

\- \*\*Y\*\* la entrega conserva el repartidor original



\### Escenario 3: Estado inválido (INV-02 e INV-05)

\- \*\*Dado\*\* una entrega en estado `EnRuta`

\- \*\*Cuando\*\* se intenta asignar un repartidor

\- \*\*Entonces\*\* el sistema rechaza la operación por estado inválido

\- \*\*Y\*\* el estado de la entrega no cambia



\### Escenario 4: Ruta saturada (INV-03)

\- \*\*Dado\*\* un repartidor cuya ruta ya alcanzó su capacidad máxima

\- \*\*Cuando\*\* se intenta asignarle una nueva entrega

\- \*\*Entonces\*\* el sistema rechaza la operación con el error "Ruta saturada"

\- \*\*Y\*\* la entrega sigue en `ListaParaDespacho`



\### Escenario 5: Repartidor no disponible (INV-04)

\- \*\*Dado\*\* un repartidor fuera de turno

\- \*\*Cuando\*\* se intenta asignarle una entrega

\- \*\*Entonces\*\* el sistema rechaza la operación con el error "Repartidor no disponible"

\- \*\*Y\*\* no se modifica la entrega ni se registra asignación



\### Escenario 6: Entrega inexistente

\- \*\*Dado\*\* un identificador de entrega que no existe

\- \*\*Cuando\*\* se solicita la asignación

\- \*\*Entonces\*\* el sistema responde "Entrega no encontrada"

\- \*\*Y\*\* no se consulta disponibilidad ni se guarda nada



\### Escenario 7: Cancelación sin motivo (INV-06)

\- \*\*Dado\*\* una entrega en estado `Asignada`

\- \*\*Cuando\*\* se intenta cancelar sin indicar motivo

\- \*\*Entonces\*\* el sistema rechaza la cancelación

\- \*\*Y\*\* la entrega sigue en `Asignada`



\### Escenario 8: Falla del servicio de disponibilidad

\- \*\*Dado\*\* que el servicio externo de disponibilidad no responde

\- \*\*Cuando\*\* se intenta asignar un repartidor

\- \*\*Entonces\*\* el sistema informa "No se pudo verificar la disponibilidad"

\- \*\*Y\*\* la entrega no cambia de estado (no se asigna a ciegas)



\## Trazabilidad: escenario → invariante



| Escenario | Invariantes comprobadas |

|-----------|-------------------------|

| 1 | INV-07 (y el flujo completo) |

| 2 | INV-01 |

| 3 | INV-02, INV-05 |

| 4 | INV-03 |

| 5 | INV-04 |

| 6 | Precondición del caso de uso |

| 7 | INV-06 |

| 8 | Resiliencia ante falla externa (RNF-02) |

