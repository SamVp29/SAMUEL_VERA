# Microservicio: Gestión de Usuarios (Users.API)

## Descripción General
Este microservicio es responsable de la administración del catálogo de usuarios del sistema. Está diseñado siguiendo una arquitectura de 3 capas (Controladores, Servicios y Acceso a Datos), lo que permite separar claramente la lógica de negocio del enrutamiento HTTP y las operaciones de base de datos.

## Responsabilidades
- Registrar nuevos usuarios.
- Consultar la lista de usuarios existentes.
- Proveer datos al sistema de distribución (WorkItems.API) mediante la referencia del nombre de usuario.

## Tecnologías Utilizadas
- **.NET 8 (Web API)**
- **Entity Framework Core**
- **SQLite** (Base de datos local para facilitar la prueba, el archivo generado será `usuarios.db`)
- **Swagger** (Documentación interactiva de la API)

## Estructura (3 Capas)
- `Controllers/`: Manejo de peticiones HTTP.
- `Services/`: Contratos (Interfaces) y la implementación de la lógica de negocio.
- `Data/`: Contexto de Entity Framework.
- `Models/`: Entidades de dominio.

## Endpoints Principales

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/usuarios` | Devuelve todos los usuarios registrados. |
| `POST` | `/api/usuarios` | Crea un nuevo usuario. |

*Nota: A través del API Gateway, estos endpoints son accesibles mediante `/gateway/usuarios`.*
