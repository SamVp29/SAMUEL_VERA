using Users.API.Models;

namespace Users.API.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
    }
}
