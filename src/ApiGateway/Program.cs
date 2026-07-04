using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Interceptar rutas base antes de Ocelot para dar info del gateway
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"servicio\": \"Api Gateway\", \"rutas_disponibles\": [\"/gateway/usuarios\", \"/gateway/items\"]}");
        return;
    }
    if (context.Request.Path == "/health")
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"estado\": \"activo\"}");
        return;
    }
    await next();
});

await app.UseOcelot();
app.Run();
