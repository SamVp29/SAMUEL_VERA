using Microsoft.EntityFrameworkCore;
using Users.API.Data;
using Users.API.Models;

namespace Users.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuariosDbContext _contexto;

        public UsuarioService(UsuariosDbContext contexto)
        {
            _contexto = contexto;
        }
        
        public async Task<List<Usuario>> ObtenerUsuariosAsync() 
        {
            // TODO: Agregar paginación si la lista de usuarios crece demasiado
            return await _contexto.Usuarios.ToListAsync();
        }
        
        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            usuario.Id = Guid.NewGuid();
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            
            return usuario;
        }
    }
}
