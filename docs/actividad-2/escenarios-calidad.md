\# Escenarios de calidad



Los escenarios de calidad describen \*\*cómo debe comportarse el sistema\*\* en situaciones concretas. Cada escenario incluye: estímulo, fuente, artefacto, entorno, respuesta y medida.



\---



\## Escenario 1: Rendimiento en consulta de estado



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Un cliente consulta el estado de su pedido desde la app. |

| \*\*Fuente\*\* | Cliente final. |

| \*\*Artefacto\*\* | Módulo de rastreo de pedidos. |

| \*\*Entorno\*\* | Operación normal, con hasta 500 usuarios concurrentes. |

| \*\*Respuesta\*\* | El sistema devuelve el estado actualizado del pedido. |

| \*\*Medida\*\* | La respuesta debe llegar en \*\*menos de 2 segundos\*\* en el 95% de las consultas. |



\---



\## Escenario 2: Disponibilidad en horario operativo



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Falla un componente interno del sistema. |

| \*\*Fuente\*\* | Infraestructura / hardware. |

| \*\*Artefacto\*\* | Sistema completo. |

| \*\*Entorno\*\* | Horario operativo (6:00 - 23:00). |

| \*\*Respuesta\*\* | El sistema se recupera o sigue operando sin interrupción perceptible. |

| \*\*Medida\*\* | Disponibilidad mínima del \*\*99%\*\* en horario operativo (máximo \~10 minutos de caída al mes). |



\---



\## Escenario 3: Seguridad en transmisión de datos



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Un atacante intenta interceptar datos de ubicación de un repartidor. |

| \*\*Fuente\*\* | Actor malicioso externo. |

| \*\*Artefacto\*\* | Canal de comunicación entre repartidor y sistema. |

| \*\*Entorno\*\* | Red móvil pública. |

| \*\*Respuesta\*\* | El sistema cifra la información y rechaza cualquier conexión no autenticada. |

| \*\*Medida\*\* | \*\*100%\*\* de las comunicaciones deben usar HTTPS/TLS 1.2 o superior. Datos sensibles cifrados en reposo con AES-256. |



\---



\## Escenario 4: Escalabilidad en día pico



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Llega un volumen inesperado de pedidos (Black Friday). |

| \*\*Fuente\*\* | Canales de venta (web, app, WhatsApp). |

| \*\*Artefacto\*\* | Módulo de registro de pedidos. |

| \*\*Entorno\*\* | Día de alta demanda, 5x el tráfico normal. |

| \*\*Respuesta\*\* | El sistema procesa los pedidos sin degradación significativa. |

| \*\*Medida\*\* | Soportar \*\*1.000 transacciones por hora\*\* sin superar los 3 segundos de tiempo de respuesta. |



\---



\## Escenario 5: Mantenibilidad ante cambio de reglas



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | El negocio agrega un nuevo estado de pedido ("En devolución"). |

| \*\*Fuente\*\* | Equipo de negocio / product owner. |

| \*\*Artefacto\*\* | Módulo de gestión de pedidos. |

| \*\*Entorno\*\* | Operación normal. |

| \*\*Respuesta\*\* | El equipo de desarrollo implementa el cambio sin afectar otros módulos. |

| \*\*Medida\*\* | El cambio debe requerir modificar \*\*solo 1 módulo\*\* y desplegarse en \*\*menos de 1 día\*\*. |



\---



\## Escenario 6: Trazabilidad de acciones



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Ocurre un incidente con un pedido (paquete perdido). |

| \*\*Fuente\*\* | Auditoría interna / reclamo de cliente. |

| \*\*Artefacto\*\* | Registro de auditoría del sistema. |

| \*\*Entorno\*\* | Investigación posterior al incidente. |

| \*\*Respuesta\*\* | El sistema puede reconstruir toda la línea de tiempo del pedido. |

| \*\*Medida\*\* | El \*\*100%\*\* de las acciones sobre un pedido quedan registradas con fecha, hora, usuario y acción. |



\---



\## Escenario 7: Usabilidad para repartidores



| Elemento | Descripción |

|----------|-------------|

| \*\*Estímulo\*\* | Un repartidor necesita actualizar el estado de una entrega. |

| \*\*Fuente\*\* | Repartidor en campo. |

| \*\*Artefacto\*\* | Interfaz móvil del repartidor. |

| \*\*Entorno\*\* | En la calle, con una mano, posiblemente con guantes. |

| \*\*Respuesta\*\* | El repartidor completa la actualización sin errores. |

| \*\*Medida\*\* | La acción debe completarse en \*\*menos de 3 clics\*\* y \*\*menos de 15 segundos\*\*. |

