\# Architecture.md



\## 1. Propósito del sistema



LogiControl centraliza la gestión logística de última milla: pedidos, inventario, entregas e incidencias, con trazabilidad en tiempo real.



\## 2. Decisiones arquitectónicas clave



| ADR | Decisión |

|-----|----------|

| ADR-001 | Monolito modular con límites claros |

| ADR-002 | Diseño por capas con dominio protegido, puertos y adaptadores (ver `docs/actividad-3`) |



\## 3. Estilo y estructura



Monolito modular. Cada módulo se organiza internamente en tres capas:



```text

/src

&#x20; /Domain          Entidades, objetos de valor, invariantes (no depende de nada)

&#x20; /Application     Casos de uso, DTOs, contratos y puertos (depende solo de Domain)

&#x20; /Infrastructure  Adaptadores: BD, APIs externas, notificaciones (depende de Application y Domain)

/tests             Pruebas unitarias del dominio e integración del flujo

```



\## 4. Regla de dependencias



Las dependencias apuntan siempre hacia el dominio. Domain no referencia a Application ni a Infrastructure. Application define interfaces (puertos); Infrastructure las implementa.



\## 5. Contextos delimitados



| Contexto | Tipo | Relación |

|----------|------|----------|

| Entregas | Núcleo | Consume eventos de Pedidos, consulta Flota, publica eventos |

| Pedidos | Soporte | Upstream de Entregas |

| Flota / Disponibilidad | Soporte | Se accede mediante capa anticorrupción |

| Notificaciones | Genérico | Suscriptor de eventos |

| Auditoría | Genérico | Recibe registros de trazabilidad |



Detalle en `docs/actividad-3/05-contextos-contratos.md`.



\## 5.1 Límites que no se cruzan



\- Un módulo no lee las tablas de otro; se comunica por eventos y contratos.

\- El dominio no usa `HttpClient`, `SqlConnection` ni clases de un proveedor.

\- Los casos de uso no conocen HTTP ni SQL.



\## 6. Reglas críticas (invariantes)



Viven en el dominio (`Delivery`, `Route`) y se verifican con escenarios Dado-Cuando-Entonces. Ver `docs/actividad-3/04-invariantes-escenarios.md`.



\## 7. Atributos de calidad que guían el diseño



Mantenibilidad (RNF-05), trazabilidad (RNF-08), evolución (RNF-07) y disponibilidad (RNF-02). Ver Actividad 2.



\## 8. Diagramas



Diagramas C4 (contexto, contenedores, componentes) en `docs/actividad-3/07-diagrama-c4.md`.



\## 9. Riesgos y trade-offs conocidos



\- Más archivos e interfaces que un CRUD directo.

\- Requiere disciplina para mantener los límites entre capas y módulos.

\- Un único despliegue implica escalar todo junto (aceptado en ADR-001).

