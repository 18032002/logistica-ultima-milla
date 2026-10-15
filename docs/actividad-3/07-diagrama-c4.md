\# Diagrama C4 (actualizado en Actividad 3)



\## Nivel 1: Contexto del sistema



```mermaid

flowchart TB

&#x20;   Despachador(\["Despachador"])

&#x20;   Repartidor(\["Repartidor"])

&#x20;   Cliente(\["Cliente"])

&#x20;   Soporte(\["Soporte al cliente"])



&#x20;   Sistema\["LogiControl<br>Plataforma logística de última milla"]



&#x20;   Pagos\["Sistema de Pagos<br>(externo)"]

&#x20;   Geo\["Servicio de Geolocalización<br>(externo)"]

&#x20;   Flota\["Servicio de Flota / Disponibilidad<br>(externo o interno)"]

&#x20;   Notif\["Notificaciones correo/SMS<br>(externo)"]

&#x20;   ERP\["ERP / CRM<br>(interno)"]



&#x20;   Despachador -->|"Asigna repartidores"| Sistema

&#x20;   Repartidor -->|"Recibe entregas, actualiza estado"| Sistema

&#x20;   Cliente -->|"Consulta estado"| Sistema

&#x20;   Soporte -->|"Gestiona incidencias"| Sistema



&#x20;   Sistema -->|"Valida pagos"| Pagos

&#x20;   Sistema -->|"Calcula rutas"| Geo

&#x20;   Sistema -->|"Consulta disponibilidad"| Flota

&#x20;   Sistema -->|"Envía mensajes"| Notif

&#x20;   Sistema -->|"Sincroniza clientes y stock"| ERP

```



\## Nivel 2: Contenedores



Por la decisión del ADR-001 es \*\*un solo despliegue\*\* (monolito modular), con una base de datos.



```mermaid

flowchart TB

&#x20;   Usuarios(\["Despachador / Repartidor / Cliente / Soporte"])



&#x20;   subgraph LogiControl\["LogiControl"]

&#x20;       Web\["Aplicación web y móvil<br>(interfaz)"]

&#x20;       Monolito\["Monolito modular<br>Módulos: Pedidos, Inventario,<br>Entregas, Incidencias,<br>Notificaciones, Integración"]

&#x20;       BD\[("Base de datos única")]

&#x20;   end



&#x20;   Externos\["Sistemas externos<br>(Pagos, Geo, Flota, Notificaciones, ERP)"]



&#x20;   Usuarios --> Web

&#x20;   Web -->|"HTTPS"| Monolito

&#x20;   Monolito --> BD

&#x20;   Monolito -->|"Adaptadores"| Externos

```



\## Nivel 3: Componentes del módulo Entregas



```mermaid

flowchart TB

&#x20;   subgraph Entregas\["Módulo Entregas"]

&#x20;       subgraph Dominio\["Domain"]

&#x20;           Delivery\["Delivery<br>(raíz de agregado)"]

&#x20;           Route\["Route"]

&#x20;           VO\["Objetos de valor<br>DeliveryId, CourierId,<br>DeliveryAddress, DeliveryStatus..."]

&#x20;       end

&#x20;       subgraph Aplicacion\["Application"]

&#x20;           UC\["AssignCourierUseCase"]

&#x20;           Puertos\["Puertos<br>IDeliveryRepository<br>ICourierAvailabilityPort<br>IAuditLog, IEventPublisher, IClock"]

&#x20;       end

&#x20;       subgraph Infra\["Infrastructure"]

&#x20;           Repo\["Adaptador de repositorio"]

&#x20;           FlotaAd\["Adaptador de flota (ACL)"]

&#x20;           AuditAd\["Adaptador de auditoría"]

&#x20;           PubAd\["Adaptador de eventos"]

&#x20;       end

&#x20;       Controlador\["Entrada: controlador / API"]

&#x20;   end



&#x20;   Controlador --> UC

&#x20;   UC --> Delivery

&#x20;   UC --> Puertos

&#x20;   Repo -.->|"implementa"| Puertos

&#x20;   FlotaAd -.->|"implementa"| Puertos

&#x20;   AuditAd -.->|"implementa"| Puertos

&#x20;   PubAd -.->|"implementa"| Puertos

```



> Las flechas punteadas indican \*\*inversión de dependencias\*\*: la infraestructura implementa lo que la aplicación define.

