using Microsoft.EntityFrameworkCore;
using Users.API.Data;
using Users.API.Models;
using Users.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuramos SQLite en memoria o local para efectos prácticos de la prueba
builder.Services.AddDbContext<UsuariosDbContext>(opciones => 
    opciones.UseSqlite("Data Source=usuarios.db"));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Inicializamos data para tener con qué probar
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
    contexto.Database.EnsureCreated(); 

    if (!contexto.Usuarios.Any())
    {
        contexto.Usuarios.AddRange(
            new Usuario { Id = Guid.NewGuid(), NombreUsuario = "UsuarioA", NombreCompleto = "Usuario A" },
            new Usuario { Id = Guid.NewGuid(), NombreUsuario = "UsuarioB", NombreCompleto = "Usuario B" }
        );
        contexto.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
