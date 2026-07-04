using Microsoft.EntityFrameworkCore;
using WorkItems.API.Data;
using WorkItems.API.Models;
using WorkItems.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Usamos SQLite local para que haya algo de persistencia durante las pruebas sin configurar SQL Server
builder.Services.AddDbContext<ItemsTrabajoDbContext>(opciones => 
    opciones.UseSqlite("Data Source=itemstrabajo.db"));

builder.Services.AddScoped<IItemTrabajoService, ItemTrabajoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Poblar datos iniciales basados en el caso práctico para agilizar las pruebas
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<ItemsTrabajoDbContext>();
    contexto.Database.EnsureCreated();

    if (!contexto.ItemsTrabajo.Any())
    {
        contexto.ItemsTrabajo.AddRange(
            new ItemTrabajo { Id = Guid.NewGuid(), Titulo = "Tarea A1", Relevancia = RelevanciaItem.Alta, FechaEntrega = DateTime.UtcNow.AddDays(5), Estado = EstadoItem.Pendiente, NombreUsuarioAsignado = "UsuarioA" },
            new ItemTrabajo { Id = Guid.NewGuid(), Titulo = "Tarea A2", Relevancia = RelevanciaItem.Alta, FechaEntrega = DateTime.UtcNow.AddDays(10), Estado = EstadoItem.Pendiente, NombreUsuarioAsignado = "UsuarioA" },
            new ItemTrabajo { Id = Guid.NewGuid(), Titulo = "Tarea A3", Relevancia = RelevanciaItem.Baja, FechaEntrega = DateTime.UtcNow.AddDays(7), Estado = EstadoItem.Pendiente, NombreUsuarioAsignado = "UsuarioA" },
            new ItemTrabajo { Id = Guid.NewGuid(), Titulo = "Tarea B1", Relevancia = RelevanciaItem.Baja, FechaEntrega = DateTime.UtcNow.AddDays(15), Estado = EstadoItem.Pendiente, NombreUsuarioAsignado = "UsuarioB" }
        );
        contexto.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
