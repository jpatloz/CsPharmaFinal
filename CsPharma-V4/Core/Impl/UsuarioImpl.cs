using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;

namespace CsPharma_V4.Core.Impl
{
    public class UsuarioImpl : UsuarioRepository
    {
        private readonly LoginContexto _context;

        public UsuarioImpl(LoginContexto context)
        {
            _context = context;
        }

        public User ActualizarUsuario(User usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public User GetUsuario(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<User> GetUsuarios()
        {
            return _context.Users.ToList();
        }
    }
}
