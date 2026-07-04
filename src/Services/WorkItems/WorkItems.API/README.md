# Microservicio: Ítems de Trabajo (WorkItems.API)

## Descripción General
Este es el microservicio central de la prueba técnica. Se encarga de gestionar los ítems de trabajo y ejecuta el **Algoritmo de Distribución y Asignación Automática** requerido por el caso práctico.

## Algoritmo de Distribución (Reglas Implementadas)
La lógica de asignación (ubicada en `Services/ItemTrabajoService.cs`) respeta estrictamente las reglas de negocio solicitadas:

1. **Urgencia por Fecha de Entrega:** Si un nuevo ítem tiene fecha de entrega próxima a vencer (menos de 3 días desde la fecha actual), se asigna al usuario con menor cantidad de ítems en total, independientemente de la relevancia.
2. **Saturación:** Si un usuario tiene más de 3 ítems pendientes clasificados como "Altamente Relevantes", se le considera saturado y es excluido de la distribución (a menos que todos los usuarios estén saturados).
3. **Distribución Estándar:** Si el ítem no es urgente, se asignará al usuario no saturado que posea la menor cantidad de ítems pendientes.
4. **Ordenación:** Al consultar los pendientes de un usuario, la lista se retorna ordenada primero por Fecha de Entrega y luego de forma descendente por Relevancia.

## Tecnologías Utilizadas
- **.NET 8 (Web API)**
- **Entity Framework Core**
- **SQLite** (Base de datos local para la persistencia, archivo: `itemstrabajo.db`)

## Endpoints Principales

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/itemstrabajo` | Obtiene el listado de todos los ítems registrados (sin filtros). |
| `GET` | `/api/itemstrabajo/estadisticas?usuarios=UsuarioA,UsuarioB` | Retorna el reporte estadístico de completados y pendientes por usuario. |
| `GET` | `/api/itemstrabajo/pendientes/{usuario}` | Retorna la lista de ítems pendientes de un usuario, debidamente ordenada. |
| `PATCH` | `/api/itemstrabajo/{id}/completar` | Cambia el estado de un ítem a 'Completado'. |
| `POST` | `/api/itemstrabajo/asignar-auto` | Ejecuta el algoritmo de distribución. Recibe un Ítem y una lista de usuarios candidatos. |

*Nota: A través del API Gateway, estos endpoints son accesibles mediante `/gateway/items`.*
