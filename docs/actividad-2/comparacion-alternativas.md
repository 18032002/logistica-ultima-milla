\# Comparación de alternativas estructurales



\## Alternativas consideradas



\### Opción A: Monolito modular con límites claros



Un solo despliegue, pero internamente organizado en módulos bien definidos (pedidos, inventario, entregas, incidencias, notificaciones) con interfaces claras entre ellos.



\*\*Ventajas:\*\*

\- Más rápido de construir para una operación con crecimiento moderado.

\- Permite definir dominios por pedido, inventario, entregas e incidencias.

\- Reduce complejidad operativa inicial (un solo despliegue, una sola base de datos).

\- Facilita la trazabilidad y el mantenimiento al tener todo en un mismo lugar.

\- Menor costo de infraestructura y DevOps.



\*\*Desventajas:\*\*

\- Puede convertirse en una base de código acoplada si no se definen límites disciplinadamente.

\- El crecimiento de la operación puede exigir refactorización hacia servicios más adelante.

\- Escalar un módulo específico implica escalar toda la aplicación.



\---



\### Opción B: Arquitectura distribuida por dominios y servicios (microservicios)



Múltiples servicios independientes (pedidos, inventario, rutas, notificaciones, incidencias), cada uno con su propia base de datos, desplegados por separado y comunicados por APIs o eventos.



\*\*Ventajas:\*\*

\- Separa mejor inventario, entrega y soporte en equipos autónomos.

\- Permite crecimiento independiente por módulo o equipo.

\- Facilita integración con varios sistemas externos (pagos, geolocalización, ERP).

\- Mayor tolerancia a fallos: si cae un servicio, los demás siguen operando.



\*\*Desventajas:\*\*

\- Aumenta la complejidad operativa (múltiples despliegues, monitoreo, logs distribuidos).

\- Exige más coordinación técnica y más infraestructura (contenedores, orquestación, service mesh).

\- Puede sobrediseñarse si el negocio aún no lo requiere.

\- Mayor dificultad para mantener consistencia de datos entre servicios.

\- Costos de DevOps y cloud significativamente mayores.



\---



\## Tabla comparativa con criterios de evaluación



| Criterio | Opción A: Monolito Modular | Opción B: Distribuido (Microservicios) |

|----------|----------------------------|----------------------------------------|

| \*\*Complejidad\*\* | Baja (1 despliegue) | Alta (múltiples servicios) |

| \*\*Costo inicial\*\* | Bajo | Alto |

| \*\*Costo operativo continuo\*\* | Bajo | Alto (infraestructura + DevOps) |

| \*\*Rapidez de implementación\*\* | Alta (semanas) | Baja (meses) |

| \*\*Facilidad de evolución\*\* | Media (requiere disciplina modular) | Alta (servicios independientes) |

| \*\*Riesgo operativo\*\* | Bajo | Alto (fallos en red, consistencia, versionado) |

| \*\*Alineación con negocio actual\*\* | Alta | Media (sobredimensionado para la etapa actual) |

| \*\*Escalabilidad\*\* | Media (escala todo el monolito) | Alta (escala por servicio) |

| \*\*Trazabilidad\*\* | Alta (base de datos única) | Compleja (datos distribuidos) |

| \*\*Equipo necesario\*\* | Pequeño (2-4 devs) | Grande (varios equipos + DevOps) |



\---



\## Conclusión de la comparación



Para la \*\*etapa actual del negocio\*\* (crecimiento moderado, equipo pequeño, necesidad de trazabilidad inmediata), la \*\*Opción A (Monolito Modular)\*\* es la más apropiada porque:



1\. Reduce el riesgo operativo y el costo inicial.

2\. Permite tener una fuente única de verdad sobre los pedidos.

3\. Facilita la trazabilidad, que es uno de los requisitos críticos de la Actividad 1.

4\. No introduce complejidad innecesaria antes de tener evidencia de que el crecimiento la justifica.



La Opción B no se descarta para el futuro, pero solo debería adoptarse cuando el volumen de pedidos, el tamaño del equipo o la necesidad de escalar módulos específicos lo justifiquen.

