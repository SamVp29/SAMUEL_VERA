using WorkItems.API.Models;

namespace WorkItems.API.Services
{
    public interface IItemTrabajoService
    {
        Task<List<ItemTrabajo>> ObtenerTodosAsync();
        Task<ItemTrabajo> CrearYAsignarAsync(ItemTrabajo nuevoItem, List<string> usuariosDisponibles);
        Task<object> ObtenerEstadisticasUsuariosAsync(List<string> nombresUsuarios);
        Task<List<ItemTrabajo>> ObtenerItemsPendientesOrdenadosAsync(string nombreUsuario);
        Task CompletarItemAsync(Guid id);
    }
}
