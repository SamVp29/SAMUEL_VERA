# Prueba Técnica - Gestión de Ítems de Trabajo (Microservicios)

## Arquitectura de la Solución

Esta solución implementa una arquitectura basada en **Microservicios** en .NET 8, siguiendo buenas prácticas de separación de responsabilidades y estructurada en 3 capas (Controladores, Servicios, Datos). 

La solución se compone de tres proyectos principales:

1. **ApiGateway:** Un proxy inverso utilizando Ocelot. Actúa como el único punto de entrada (puerto `5000`) y enruta las peticiones hacia los microservicios correspondientes (`/gateway/usuarios` y `/gateway/items`).
2. **Users.API (Gestión de Usuarios):** Microservicio encargado de administrar a los usuarios. (Puerto `5001`).
3. **WorkItems.API (Ítems de Trabajo):** Microservicio encargado de la asignación y gestión de los ítems, implementando el algoritmo de distribución requerido en el caso práctico. (Puerto `5002`).

## Requisitos Previos

- .NET 8 SDK
- Visual Studio 2022 o Visual Studio Code

## ¿Cómo ejecutar el proyecto?

### Opción 1: Visual Studio 2022
1. Abre la solución `SAMUEL_VERA.sln` en Visual Studio 2022.
2. En la barra superior, asegúrate de tener configurado "Múltiples proyectos de inicio" (Multiple Startup Projects).
3. Establece `ApiGateway`, `Users.API` y `WorkItems.API` para que inicien (Start).
4. Presiona `F5` para compilar y ejecutar todo.

### Opción 2: Línea de Comandos (CLI)
Si prefieres usar la terminal, abre 3 pestañas y ejecuta los siguientes comandos desde la raíz del repositorio:

```bash
# Terminal 1
dotnet run --project src/ApiGateway/ApiGateway.csproj

# Terminal 2
dotnet run --project src/Services/Users/Users.API/Users.API.csproj

# Terminal 3
dotnet run --project src/Services/WorkItems/WorkItems.API/WorkItems.API.csproj
```

## Pruebas de Endpoints y Swagger

Dado que se trata de microservicios independientes, la interfaz de **Swagger** está disponible directamente en los puertos de cada servicio:

- **Swagger Usuarios:** `http://localhost:5001/swagger`
- **Swagger Ítems de Trabajo:** `http://localhost:5002/swagger`

Para probar la integración a través del **API Gateway**, puedes realizar peticiones directamente al puerto `5000`:
- Obtener todos los usuarios: `GET http://localhost:5000/gateway/usuarios`
- Obtener todos los ítems: `GET http://localhost:5000/gateway/items`

## Persistencia de Datos

Para facilitar la evaluación de esta prueba y no requerir dependencias externas como SQL Server o Docker, los microservicios utilizan **SQLite** como base de datos local.
Al iniciarse, cada servicio asegura la creación de su base de datos respectiva (`usuarios.db` y `itemstrabajo.db`) y genera **datos de prueba iniciales (Semilla)** para poder probar inmediatamente el algoritmo de distribución.
#