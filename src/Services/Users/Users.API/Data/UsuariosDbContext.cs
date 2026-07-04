using Microsoft.EntityFrameworkCore;
using Users.API.Models;

namespace Users.API.Data
{
    public class UsuariosDbContext : DbContext
    {
        public UsuariosDbContext(DbContextOptions<UsuariosDbContext> opciones) : base(opciones) { }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
