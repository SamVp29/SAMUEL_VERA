using Microsoft.AspNetCore.Mvc;
using Users.API.Models;
using Users.API.Services;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            // TODO: Implementar validación de campos obligatorios
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                return BadRequest("El nombre de usuario es obligatorio");

            var creado = await _usuarioService.CrearUsuarioAsync(usuario);
            return Ok(creado);
        }
    }
}
