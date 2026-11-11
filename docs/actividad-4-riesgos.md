\# Registro de Riesgos, Observabilidad y Resultados (Actividad 4)



\## 1. Riesgos Operativos y de Seguridad



| Riesgo | Impacto | Mitigación Implementada | Evidencia de Verificación |

|--------|---------|-------------------------|---------------------------|

| \*\*Asignación duplicada de repartidor\*\* (Race Condition) | Un repartidor recibe dos entregas al mismo tiempo o una entrega se asigna dos veces. | Invariante en el Dominio (`AssignCourier` lanza excepción si ya está asignado). | Prueba unitaria `AssignCourier\_Should\_ThrowException\_When\_DeliveryAlreadyHasCourier`. |

| \*\*Caída del servicio de repartidores (API externa)\*\* | El sistema intenta asignar y falla estrepitosamente. | Uso del puerto `ICourierAvailabilityPort` y simulación de fallos controlados. | Prueba de integración `ExecuteAsync\_Should\_ThrowException\_When\_CourierIsNotAvailable`. |

| \*\*Fuga de información sensible en Logs\*\* | Se registran direcciones o nombres de clientes en texto plano. | Los logs solo registran IDs técnicos (`DeliveryId`, `CourierId`), nunca direcciones ni datos personales. | Revisión de código en `DeliveriesController`. |

| \*\*Acoplamiento indebido del Dominio\*\* | Un desarrollador importa Entity Framework o HTTP en la capa de Dominio. | Fitness Function con `NetArchTest`. | Prueba `Domain\_Should\_Not\_DependOn\_Application\_Or\_Infrastructure`. |



\## 2. Señales de Observabilidad



\- \*\*Logs Estructurados:\*\* Se configuró `ILogger` en la capa de entrada. En producción, esto debería enviarse a herramientas como Application Insights, ELK o Datadog.

\- \*\*Niveles de Log:\*\*

&#x20; - `Information`: Flujo normal (inicio y fin de asignación).

&#x20; - `Warning`: Errores de negocio controlados (repartidor no disponible, entrega ya asignada).

&#x20; - `Error`: Excepciones no controladas (fallos de BD, errores internos).

\- \*\*Trazabilidad:\*\* Cada log incluye el `DeliveryId` para poder seguir el recorrido de una entrega específica sin exponer la dirección.



\## 3. Resultados de las Pruebas



\- \*\*Total de Pruebas:\*\* 8

\- \*\*Superadas:\*\* 8

\- \*\*Errores:\*\* 0

\- \*\*Cobertura de Invariantes:\*\* Se probaron las reglas INV-01 (entrega ya asignada) y INV-03 (estado inválido).

\- \*\*Comprobación de Arquitectura:\*\* Se validó con Fitness Functions que las dependencias apuntan hacia adentro.



\## 4. Riesgos Residuales y Próximos Pasos



\- \*\*Riesgo Residual 1:\*\* El repositorio actual es en memoria (`InMemoryDeliveryRepository`), por lo que los datos se pierden al reiniciar la API.

&#x20; - \*Próximo paso:\* Implementar un adaptador real con Entity Framework Core + SQL Server / PostgreSQL en la Actividad 4 (fase final).

\- \*\*Riesgo Residual 2:\*\* La disponibilidad del repartidor es un "Mock" que no consulta una API real.

&#x20; - \*Próximo paso:\* Implementar `HttpCourierAvailabilityAdapter` conectando con el servicio real de repartidores.

\- \*\*Riesgo Residual 3:\*\* No hay autenticación en la API.

&#x20; - \*Próximo paso:\* Configurar JWT Bearer Tokens para proteger los endpoints.



