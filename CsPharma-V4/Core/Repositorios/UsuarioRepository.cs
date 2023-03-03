using CsPharma_V4.Areas.Identity.Data;

namespace CsPharma_V4.Core.Repositorios
{
    public interface UsuarioRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
    }
}
