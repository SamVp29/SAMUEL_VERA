using Microsoft.AspNetCore.Mvc;
using WorkItems.API.Models;
using WorkItems.API.Services;

namespace WorkItems.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsTrabajoController : ControllerBase
    {
        private readonly IItemTrabajoService _itemTrabajoService;

        public ItemsTrabajoController(IItemTrabajoService itemTrabajoService)
        {
            _itemTrabajoService = itemTrabajoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _itemTrabajoService.ObtenerTodosAsync());

        [HttpGet("estadisticas")]
        public async Task<IActionResult> GetEstadisticas([FromQuery] string usuarios)
        {
            if (string.IsNullOrEmpty(usuarios)) 
                return BadRequest("Faltan usuarios en la petición");
            
            var listaUsuarios = usuarios.Split(',').ToList();
            return Ok(await _itemTrabajoService.ObtenerEstadisticasUsuariosAsync(listaUsuarios));
        }

        [HttpGet("pendientes/{usuario}")]
        public async Task<IActionResult> GetPendientes(string usuario)
        {
            return Ok(await _itemTrabajoService.ObtenerItemsPendientesOrdenadosAsync(usuario));
        }

        [HttpPatch("{id}/completar")]
        public async Task<IActionResult> CompletarItem(Guid id)
        {
            await _itemTrabajoService.CompletarItemAsync(id);
            return Ok(new { mensaje = "Ítem marcado como completado." });
        }

        public class PeticionCrearItem
        {
            public ItemTrabajo Item { get; set; } = new();
            public List<string> UsuariosCandidatos { get; set; } = new();
        }

        [HttpPost("asignar-auto")]
        public async Task<IActionResult> CrearYAsignar([FromBody] PeticionCrearItem peticion)
        {
            // TODO: Usar FluentValidation para validar la petición en el futuro
            if (peticion.Item == null || !peticion.UsuariosCandidatos.Any())
                return BadRequest("Petición inválida");

            var creado = await _itemTrabajoService.CrearYAsignarAsync(peticion.Item, peticion.UsuariosCandidatos);
            
            // Requerimiento: "Ordenar la lista de pendientes por usuario después de cada asignación"
            var listaOrdenada = await _itemTrabajoService.ObtenerItemsPendientesOrdenadosAsync(creado.NombreUsuarioAsignado!);

            return Ok(new 
            { 
                Mensaje = $"Ítem asignado a: {creado.NombreUsuarioAsignado}",
                ItemAsignado = creado, 
                ListaPendientesActualizada = listaOrdenada 
            });
        }
    }
}
