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

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id.ToString());
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
