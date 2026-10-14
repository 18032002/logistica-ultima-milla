\# ADR-001: Decisión estructural para la plataforma logística



\## Estado



Aceptado



\## Fecha



2025-01-15 \*(ajusta la fecha a la actual)\*



\## Contexto



La plataforma de gestión logística de última milla necesita una estructura arquitectónica que soporte los requisitos identificados en la Actividad 1 y priorizados en la Actividad 2.



Los requisitos críticos que condicionan la decisión son:



\- \*\*RF-02 (Validar inventario):\*\* impacto negocio 5, arquitectura 5.

\- \*\*RF-04 (Rastrear entrega):\*\* impacto negocio 5, arquitectura 5.

\- \*\*RF-08 (Sincronizar con ERP):\*\* impacto negocio 5, arquitectura 5.

\- \*\*RNF-02 (Disponibilidad 99%):\*\* impacto negocio 5, arquitectura 5.

\- \*\*RNF-04 (Escalabilidad):\*\* impacto arquitectura 5.

\- \*\*RNF-05 (Mantenibilidad):\*\* impacto arquitectura 5.

\- \*\*RNF-07 (Evolución):\*\* impacto arquitectura 5.



Además, el equipo de desarrollo es pequeño (2-4 personas), el negocio está en una etapa de crecimiento moderado y la trazabilidad es un requisito crítico.



\## Alternativas consideradas



1\. \*\*Opción A: Monolito modular con límites claros.\*\*

2\. \*\*Opción B: Arquitectura distribuida por dominios y servicios (microservicios).\*\*



La comparación detallada se encuentra en `comparacion-alternativas.md`.



\## Decisión



Se elige la \*\*Opción A: Monolito modular con límites claros\*\*.



El sistema se organizará internamente en módulos bien definidos:



\- \*\*Módulo de Pedidos:\*\* registro, validación y ciclo de vida del pedido.

\- \*\*Módulo de Inventario:\*\* consulta y actualización de stock en tiempo real.

\- \*\*Módulo de Entregas:\*\* asignación de rutas, rastreo y actualización de estados.

\- \*\*Módulo de Incidencias:\*\* gestión de problemas en la entrega.

\- \*\*Módulo de Notificaciones:\*\* envío de correos/SMS al cliente.

\- \*\*Módulo de Integración:\*\* comunicación con sistemas externos (pagos, geolocalización, ERP).



Cada módulo tendrá interfaces claras y responsabilidades bien delimitadas, evitando el acoplamiento.



\## Justificación



La Opción A es la más apropiada para este caso concreto porque:



1\. \*\*Reduce el riesgo operativo:\*\* un solo despliegue y una sola base de datos facilitan la trazabilidad y la consistencia de datos.

2\. \*\*Menor costo inicial y operativo:\*\* no requiere infraestructura compleja ni equipo DevOps dedicado.

3\. \*\*Rapidez de implementación:\*\* permite entregar valor al negocio en semanas, no meses.

4\. \*\*Alineación con el negocio actual:\*\* el volumen de pedidos aún no justifica la complejidad de microservicios.

5\. \*\*Facilita la mantenibilidad:\*\* con límites modulares claros, los cambios en reglas de negocio afectan solo un módulo.

6\. \*\*Permite evolución futura:\*\* si el negocio crece, los módulos pueden extraerse gradualmente hacia servicios independientes.



\## Trade-offs (compromisos asumidos)



| Beneficio | Costo / Trade-off |

|-----------|-------------------|

| Menor complejidad operativa | Escalar un módulo implica escalar toda la aplicación |

| Menor costo inicial | Si el equipo crece mucho, la coordinación en un solo código puede volverse más compleja |

| Trazabilidad centralizada | La base de datos única puede convertirse en cuello de botella si no se optimiza |

| Rapidez de implementación | Requiere disciplina para mantener los límites modulares y no caer en el acoplamiento |



\## Riesgos asociados a la decisión



| Riesgo | Mitigación |

|--------|-----------|

| Acoplamiento accidental entre módulos | Revisiones de código, pruebas de arquitectura, documentación de interfaces |

| Dificultad para escalar un módulo específico | Monitoreo de rendimiento; si un módulo se vuelve crítico, evaluar extraerlo como servicio |

| Crecimiento descontrolado del monolito | Definir métricas de tamaño y complejidad; revisar la decisión cada 6 meses |



\## Criterios verificables



Para revisar si la decisión sigue siendo válida:



\- \[ ] El sistema soporta 1.000 transacciones por hora sin degradación.

\- \[ ] El 99% de disponibilidad en horario operativo se cumple.

\- \[ ] Los cambios en reglas de negocio afectan solo 1 módulo.

\- \[ ] La trazabilidad del 100% de las acciones sobre pedidos está garantizada.

\- \[ ] El equipo puede desplegar una nueva versión en menos de 1 día.

\- \[ ] El costo de infraestructura se mantiene dentro del presupuesto.



\## Consecuencias



\- \*\*Positivas:\*\* base sólida para el diseño y construcción en las siguientes unidades; menor riesgo inicial; mayor velocidad de entrega.

\- \*\*Negativas:\*\* posible necesidad de refactorización si el negocio crece rápidamente; disciplina constante para mantener los límites modulares.

\- \*\*Revisión:\*\* esta decisión debe reevaluarse si el volumen de pedidos supera las 5.000 transacciones por hora o si el equipo de desarrollo supera las 10 personas.



\## Referencias



\- Actividad 1: Diagnóstico y contexto arquitectónico.

\- `catalogo-requisitos.md`

\- `matriz-priorizacion.md`

\- `escenarios-calidad.md`

\- `comparacion-alternativas.md`

