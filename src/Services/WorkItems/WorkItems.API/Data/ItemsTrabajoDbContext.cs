using Microsoft.EntityFrameworkCore;
using WorkItems.API.Models;

namespace WorkItems.API.Data
{
    public class ItemsTrabajoDbContext : DbContext
    {
        public ItemsTrabajoDbContext(DbContextOptions<ItemsTrabajoDbContext> opciones) : base(opciones) { }
        public DbSet<ItemTrabajo> ItemsTrabajo { get; set; }
    }
}
