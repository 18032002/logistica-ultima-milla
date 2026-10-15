\# ADR-002: Diseño de dominio, capas y puertos/adaptadores



\## Estado



Aceptado



\## Fecha



2026-10-14



\## Contexto



El ADR-001 definió un monolito modular. Dentro del módulo Entregas hay que decidir cómo organizar el negocio para que sus reglas no dependan de la base de datos, de APIs externas ni de un framework. El flujo elegido es "Asignar un repartidor a una entrega" (`AssignCourierUseCase`).



\## Decisiones



1\. \*\*Tres capas por módulo:\*\* Domain, Application e Infrastructure.

2\. \*\*Dependencias hacia adentro:\*\* Domain no depende de nada; Application solo de Domain; Infrastructure de ambas.

3\. \*\*`Delivery` como raíz de agregado\*\*, que protege las invariantes que dependen solo de su estado (INV-01, INV-02, INV-05, INV-06).

4\. \*\*Reglas que requieren información externa\*\* (disponibilidad y cupo, INV-03 e INV-04) se verifican en el caso de uso mediante un puerto y luego el dominio ejecuta la asignación.

5\. \*\*Puertos definidos en Application:\*\* `IDeliveryRepository`, `ICourierAvailabilityPort`, `IAuditLog`, `IEventPublisher`, `IClock`.

6\. \*\*Capa anticorrupción (ACL)\*\* entre Entregas y el servicio de Flota.

7\. \*\*Referencias entre agregados por identificador\*\* (`CourierId`), no por objeto.

8\. \*\*Comunicación entre contextos por eventos y contratos\*\*, sin acceso a tablas ajenas.

9\. \*\*Resultado de aplicación con códigos de error\*\* en lugar de exponer excepciones técnicas al exterior.



\## Alternativas descartadas



| Alternativa | Por qué se descartó |

|-------------|---------------------|

| CRUD directo con lógica en controladores | Las reglas quedarían dispersas y acopladas a HTTP y a la BD. |

| Modelo anémico (entidades sin comportamiento, lógica en servicios) | Cualquiera podría saltarse las invariantes al modificar campos directamente. |

| Dos capas (lógica + datos) | No separa los casos de uso del dominio ni permite cambiar proveedores sin tocar el negocio. |

| Consultar la API de flota directamente desde el caso de uso | Acopla la aplicación a un proveedor concreto y dificulta las pruebas. |

| Microservicio separado para Entregas | Contradice el ADR-001; la complejidad operativa no se justifica todavía. |

| Máquina de estados de interfaz (Video 25) | No resuelve una necesidad del caso; los estados relevantes son del dominio y ya están controlados en `Delivery`. |



\## Trade-offs



| Beneficio | Costo |

|-----------|-------|

| Reglas protegidas y testeables sin infraestructura | Más interfaces y archivos que un CRUD |

| Cambiar de proveedor tocando solo un adaptador | Mantener la traducción de la ACL |

| Contextos con modelo propio | Duplicar algunos conceptos (por ejemplo, la dirección) entre contextos |

| Resultado con códigos de error explícitos | Más código de mapeo hacia HTTP |



\## Consecuencias



\- \*\*Positivas:\*\* negocio cambiable sin depender de BD, API o framework; escenarios verificables.

\- \*\*Negativas:\*\* requiere disciplina de capas; curva de aprendizaje.

\- \*\*Revisión:\*\* reevaluar si aparece un segundo contexto con reglas igual de complejas o si se extrae Entregas como servicio.



\## Referencias



\- ADR-001, Actividades 1 y 2.

\- `docs/actividad-3/01` a `07`, y `Architecture.md`.

