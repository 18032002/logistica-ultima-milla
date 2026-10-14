\# Catálogo de requisitos - Plataforma Logística de Última Milla



\## Requisitos funcionales (RF)



| ID | Requisito | Descripción | Prioridad |

|----|-----------|-------------|-----------|

| RF-01 | Registrar pedido | El sistema debe permitir registrar un pedido desde múltiples canales (web, app, tienda física, WhatsApp). | Alta |

| RF-02 | Validar inventario | El sistema debe consultar el stock disponible en tiempo real antes de confirmar un pedido. | Alta |

| RF-03 | Asignar ruta | El sistema debe asignar automáticamente una ruta óptima al repartidor según ubicación y carga. | Alta |

| RF-04 | Rastrear entrega | El sistema debe mostrar al cliente y al operador el estado del pedido en tiempo real (preparando, en ruta, entregado). | Alta |

| RF-05 | Gestionar incidencias | El sistema debe permitir reportar y gestionar incidencias (cliente ausente, dirección errónea, paquete dañado). | Media |

| RF-06 | Notificar al cliente | El sistema debe enviar notificaciones automáticas por correo/SMS en cada cambio de estado del pedido. | Media |

| RF-07 | Consultar historial | El sistema debe permitir al cliente y al soporte consultar el historial de pedidos y entregas. | Media |

| RF-08 | Sincronizar con ERP | El sistema debe sincronizar clientes, productos y stock con el ERP/CRM interno. | Alta |

| RF-09 | Validar pago | El sistema debe comunicarse con el sistema de pagos externo para validar transacciones. | Alta |

| RF-10 | Generar reportes | El sistema debe generar reportes operativos básicos (entregas completadas, fallidas, tiempos promedio). | Baja |



\## Requisitos no funcionales (RNF)



| ID | Requisito | Descripción | Prioridad |

|----|-----------|-------------|-----------|

| RNF-01 | Rendimiento | El sistema debe responder a consultas de estado de pedido en menos de 2 segundos. | Alta |

| RNF-02 | Disponibilidad | El sistema debe estar disponible al menos el 99% del tiempo en horario operativo (6:00 - 23:00). | Alta |

| RNF-03 | Seguridad | La información sensible (datos de cliente, ubicación, pagos) debe estar cifrada en tránsito y en reposo. | Alta |

| RNF-04 | Escalabilidad | El sistema debe soportar al menos 1.000 transacciones por hora sin degradación significativa. | Media |

| RNF-05 | Mantenibilidad | Los cambios en reglas de negocio (ej: nuevos estados de pedido) no deben requerir modificar toda la aplicación. | Media |

| RNF-06 | Usabilidad | La interfaz para repartidores debe ser operable con una mano y en menos de 3 clics para actualizar una entrega. | Media |

| RNF-07 | Evolución | El sistema debe permitir añadir nuevos canales de venta o métodos de entrega sin rediseñar la arquitectura. | Media |

| RNF-08 | Trazabilidad | Toda acción sobre un pedido debe quedar registrada con fecha, hora y usuario responsable. | Alta |

