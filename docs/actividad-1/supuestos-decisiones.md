\# Supuestos y decisiones iniciales



\## Supuestos (qué asumo)



\- Los repartidores tienen smartphone con datos móviles.

\- Los clientes tienen acceso a internet para consultar estados.

\- El sistema de pagos externo responde en < 2 segundos.

\- Los operadores de tienda y almacén tienen alfabetización digital básica.

\- El equipo de soporte está disponible en horario comercial.



\## Decisiones iniciales (qué ya decido)



\- \*\*Arquitectura centralizada\*\* (monolítica) en lugar de microservicios, porque el equipo es pequeño y necesitamos control total de la trazabilidad desde el inicio.

\- \*\*Prioridad funcional:\*\* rastreo en tiempo real > reportes analíticos en la primera versión.

\- \*\*Integración estándar:\*\* usaremos servicios externos (Google Maps) para geolocalización, no desarrollaremos uno propio.



\## Incógnitas (qué aún no sé)



\- Si los repartidores serán propios o externalizados (couriers).

\- El volumen máximo de pedidos en días pico (Black Friday, Hot Sale).

\- Si el ERP interno permite integración por API o requiere archivos planos.



\## Confirmaciones pendientes (qué habrá que validar luego)



\- Entrevistar a operadores de almacén para definir flujo de \*picking\* (secuencial o por lotes).

\- Consultar con legal las políticas de privacidad para compartir ubicación en tiempo real.

\- Verificar si las rutas deben ser dinámicas (cambian en trayecto) o estáticas (fijas al inicio del día).

