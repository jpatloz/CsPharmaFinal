using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;

namespace CsPharma_V4.Core.Impl
{
    public class UsuarioImpl : UsuarioRepository
    {
        // Esta variable guarda una instancia del contexto de Login
        private readonly LoginContexto _context;

        // Constructor que recibe una instancia de LoginContexto
        public UsuarioImpl(LoginContexto context)
        {
            _context = context;
        }

        // Método que actualiza un usuario en la base de datos
        public User ActualizarUsuario(User usuario)
        {
            // Se actualiza el usuario en el contexto de Login y se guardan los cambios en la base de datos
            _context.Update(usuario);
            _context.SaveChanges();

            // Se devuelve el usuario actualizado
            return usuario;
        }

        // Método que devuelve un usuario por su id
        public User GetUsuario(string id)
        {
            // Se busca el usuario por su id en el contexto de Login
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Método que devuelve una colección de objetos User
        public ICollection<User> GetUsuarios()
        {
            // Se obtienen todos los usuarios del contexto de Login y se convierten en una lista
            return _context.Users.ToList();
        }
    }
}