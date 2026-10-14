\# Matriz de priorización de requisitos



\## Criterios de evaluación



| Criterio | Descripción |

|----------|-------------|

| \*\*Prioridad\*\* | Alta / Media / Baja según urgencia para el negocio |

| \*\*Impacto en negocio\*\* | 1 (bajo) a 5 (alto): cuánto afecta a la operación y satisfacción del cliente |

| \*\*Impacto en arquitectura\*\* | 1 (bajo) a 5 (alto): cuánto condiciona la estructura del sistema |

| \*\*Complejidad\*\* | 1 (baja) a 5 (alta): esfuerzo técnico de implementación |



\## Matriz de requisitos funcionales



| ID | Requisito | Prioridad | Impacto negocio | Impacto arquitectura | Complejidad |

|----|-----------|-----------|-----------------|----------------------|-------------|

| RF-01 | Registrar pedido | Alta | 5 | 4 | 3 |

| RF-02 | Validar inventario | Alta | 5 | 5 | 4 |

| RF-03 | Asignar ruta | Alta | 5 | 4 | 4 |

| RF-04 | Rastrear entrega | Alta | 5 | 5 | 4 |

| RF-05 | Gestionar incidencias | Media | 4 | 3 | 3 |

| RF-06 | Notificar al cliente | Media | 3 | 2 | 2 |

| RF-07 | Consultar historial | Media | 3 | 2 | 2 |

| RF-08 | Sincronizar con ERP | Alta | 5 | 5 | 4 |

| RF-09 | Validar pago | Alta | 4 | 4 | 3 |

| RF-10 | Generar reportes | Baja | 2 | 1 | 2 |



\## Matriz de requisitos no funcionales



| ID | Requisito | Prioridad | Impacto negocio | Impacto arquitectura | Complejidad |

|----|-----------|-----------|-----------------|----------------------|-------------|

| RNF-01 | Rendimiento (<2s) | Alta | 5 | 4 | 3 |

| RNF-02 | Disponibilidad (99%) | Alta | 5 | 5 | 4 |

| RNF-03 | Seguridad (cifrado) | Alta | 5 | 4 | 3 |

| RNF-04 | Escalabilidad (1000 tx/h) | Media | 3 | 5 | 4 |

| RNF-05 | Mantenibilidad | Media | 4 | 5 | 3 |

| RNF-06 | Usabilidad repartidor | Media | 4 | 2 | 2 |

| RNF-07 | Evolución (nuevos canales) | Media | 4 | 5 | 3 |

| RNF-08 | Trazabilidad | Alta | 5 | 4 | 3 |



\## Requisitos críticos (Alta prioridad + Alto impacto arquitectónico)



Estos son los requisitos que \*\*condicionan la decisión estructural\*\*:



1\. \*\*RF-02 (Validar inventario):\*\* Impacto negocio 5, arquitectura 5.

2\. \*\*RF-04 (Rastrear entrega):\*\* Impacto negocio 5, arquitectura 5.

3\. \*\*RF-08 (Sincronizar con ERP):\*\* Impacto negocio 5, arquitectura 5.

4\. \*\*RNF-02 (Disponibilidad 99%):\*\* Impacto negocio 5, arquitectura 5.

5\. \*\*RNF-04 (Escalabilidad):\*\* Impacto arquitectura 5.

6\. \*\*RNF-05 (Mantenibilidad):\*\* Impacto arquitectura 5.

7\. \*\*RNF-07 (Evolución):\*\* Impacto arquitectura 5.

