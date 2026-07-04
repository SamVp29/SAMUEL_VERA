using Microsoft.EntityFrameworkCore;
using WorkItems.API.Data;
using WorkItems.API.Models;

namespace WorkItems.API.Services
{
    public class ItemTrabajoService : IItemTrabajoService
    {
        private readonly ItemsTrabajoDbContext _contexto;

        public ItemTrabajoService(ItemsTrabajoDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<ItemTrabajo>> ObtenerTodosAsync()
        {
            return await _contexto.ItemsTrabajo.ToListAsync();
        }

        public async Task<ItemTrabajo> CrearYAsignarAsync(ItemTrabajo nuevoItem, List<string> usuariosDisponibles)
        {
            nuevoItem.Id = Guid.NewGuid();
            nuevoItem.Estado = EstadoItem.Pendiente;

            // TODO: Refactorizar a un patrón Strategy si las reglas del negocio cambian o se complican más adelante
            nuevoItem.NombreUsuarioAsignado = await DeterminarMejorUsuarioAsync(nuevoItem, usuariosDisponibles);

            _contexto.ItemsTrabajo.Add(nuevoItem);
            await _contexto.SaveChangesAsync();
            return nuevoItem;
        }

        private async Task<string?> DeterminarMejorUsuarioAsync(ItemTrabajo nuevoItem, List<string> usuariosDisponibles)
        {
            if (usuariosDisponibles == null || !usuariosDisponibles.Any())
                return null;

            var itemsPendientes = await _contexto.ItemsTrabajo
                .Where(w => w.Estado == EstadoItem.Pendiente && 
                            w.NombreUsuarioAsignado != null && 
                            usuariosDisponibles.Contains(w.NombreUsuarioAsignado))
                .ToListAsync();

            var estadisticasUsuarios = usuariosDisponibles.Select(u => new
            {
                NombreUsuario = u,
                TotalPendientes = itemsPendientes.Count(w => w.NombreUsuarioAsignado == u),
                CantidadAltaRelevancia = itemsPendientes.Count(w => w.NombreUsuarioAsignado == u && w.Relevancia == RelevanciaItem.Alta)
            }).ToList();

            // Regla: si falta poco para la entrega (< 3 días), asignar al que tenga menos ítems
            bool esUrgente = (nuevoItem.FechaEntrega.Date - DateTime.UtcNow.Date).TotalDays < 3;
            if (esUrgente)
            {
                return estadisticasUsuarios.OrderBy(u => u.TotalPendientes).First().NombreUsuario;
            }

            // Excluir a los saturados (tienen más de 3 ítems altamente relevantes)
            var usuariosNoSaturados = estadisticasUsuarios.Where(u => u.CantidadAltaRelevancia <= 3).ToList();
            
            if (!usuariosNoSaturados.Any())
            {
                // Si todos están saturados, se lo damos al que menos tenga para no dejarlo colgado
                return estadisticasUsuarios.OrderBy(u => u.TotalPendientes).First().NombreUsuario;
            }

            // Asignar al que tenga menos pendientes de los disponibles
            return usuariosNoSaturados.OrderBy(u => u.TotalPendientes).First().NombreUsuario;
        }

        public async Task<object> ObtenerEstadisticasUsuariosAsync(List<string> nombresUsuarios)
        {
            var items = await _contexto.ItemsTrabajo
                .Where(w => w.NombreUsuarioAsignado != null && nombresUsuarios.Contains(w.NombreUsuarioAsignado))
                .ToListAsync();

            return nombresUsuarios.Select(u => new
            {
                NombreUsuario = u,
                CantidadCompletados = items.Count(w => w.NombreUsuarioAsignado == u && w.Estado == EstadoItem.Completado),
                CantidadPendientes = items.Count(w => w.NombreUsuarioAsignado == u && w.Estado == EstadoItem.Pendiente),
                PendientesAltaRelevancia = items.Count(w => w.NombreUsuarioAsignado == u && w.Estado == EstadoItem.Pendiente && w.Relevancia == RelevanciaItem.Alta)
            }).ToList();
        }

        public async Task<List<ItemTrabajo>> ObtenerItemsPendientesOrdenadosAsync(string nombreUsuario)
        {
            return await _contexto.ItemsTrabajo
                .Where(w => w.NombreUsuarioAsignado == nombreUsuario && w.Estado == EstadoItem.Pendiente)
                .OrderBy(w => w.FechaEntrega)
                .ThenByDescending(w => w.Relevancia)
                .ToListAsync();
        }
        
        public async Task CompletarItemAsync(Guid id)
        {
            var item = await _contexto.ItemsTrabajo.FindAsync(id);
            if (item != null)
            {
                item.Estado = EstadoItem.Completado;
                // TODO: ¿Deberíamos guardar quién lo completó o la fecha exacta?
                await _contexto.SaveChangesAsync();
            }
        }
    }
}
