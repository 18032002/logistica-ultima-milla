\# Glosario de lenguaje ubicuo (contexto: Entregas)



\## Términos del dominio



| Término (negocio) | Nombre en código | Tipo | Definición |

|-------------------|------------------|------|------------|

| Entrega | `Delivery` | Entidad | Traslado de un pedido ya preparado hasta la dirección del cliente. Tiene identidad propia y ciclo de vida. |

| Repartidor | `Courier` | Entidad | Persona que ejecuta entregas. Tiene identidad propia. |

| Ruta | `Route` | Entidad | Conjunto ordenado de entregas que un repartidor realiza en una jornada. |

| Asignar | `AssignCourier` | Comportamiento | Vincular un repartidor a una entrega lista para despacho. |

| Reasignar | `ReassignCourier` | Comportamiento | Reemplazar el repartidor de una entrega ya asignada (fuera de esta actividad). |

| Dirección de entrega | `DeliveryAddress` | Objeto de valor | Lugar donde se entrega. Se describe solo por sus atributos. |

| Identificador de entrega | `DeliveryId` | Objeto de valor | Valor que identifica de forma única una entrega. |

| Identificador de repartidor | `CourierId` | Objeto de valor | Valor que identifica de forma única a un repartidor. |

| Estado de entrega | `DeliveryStatus` | Objeto de valor | Situación actual: ListaParaDespacho, Asignada, EnRuta, Entregada, Fallida, Cancelada. |

| Capacidad de ruta | `RouteCapacity` | Objeto de valor | Máximo de entregas que admite una ruta. |

| Ventana de entrega | `DeliveryWindow` | Objeto de valor | Rango de fecha y hora en que se promete entregar. |

| Repartidor activo | (regla) | Concepto | Repartidor actualmente vinculado a una entrega que no está cerrada. |

| Disponibilidad | `CourierAvailability` | Objeto de valor | Respuesta del servicio externo: si el repartidor puede tomar la entrega y si su ruta tiene cupo. |



\## Estados de la entrega



| Estado | Significado |

|--------|-------------|

| ListaParaDespacho | Pedido pagado y empacado, sin repartidor. |

| Asignada | Tiene repartidor activo, aún no sale. |

| EnRuta | El repartidor salió con el paquete. |

| Entregada | El cliente recibió el pedido. |

| Fallida | Intento fallido (cliente ausente, dirección errónea). |

| Cancelada | Se anuló antes de entregarse. |



\## Términos ambiguos (confirmar con el negocio)



| Término | Ambigüedad | Supuesto de trabajo | A quién confirmar |

|---------|-----------|---------------------|-------------------|

| Disponible | ¿Disponible significa "en turno", "sin entregas en curso" o "con cupo en su ruta"? | En turno y con cupo en su ruta. | Jefe de logística |

| Ruta saturada | ¿Se mide por número de entregas, peso, volumen o tiempo? | Por número máximo de entregas. | Jefe de logística |

| Repartidor propio vs. externo | ¿Los couriers externos se asignan igual que los propios? | Se tratan igual en esta actividad. | Operaciones |

| Entrega vs. pedido | ¿Un pedido puede generar varias entregas (envíos parciales)? | Un pedido genera una sola entrega. | Negocio / producto |

| Asignada vs. confirmada | ¿El repartidor debe aceptar la asignación o se impone? | Se impone, sin aceptación. | Operaciones |

| Reasignación | ¿Cuándo se permite cambiar de repartidor? | Fuera del alcance de esta actividad. | Operaciones |



\## Relación con la Actividad 1



La incógnita "¿repartidores propios o externos?" sigue abierta y aparece aquí como término ambiguo.

