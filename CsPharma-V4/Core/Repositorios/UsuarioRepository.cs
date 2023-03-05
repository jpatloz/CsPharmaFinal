using CsPharma_V4.Areas.Identity.Data;

namespace CsPharma_V4.Core.Repositorios
{
    //Interfaz donde se encuentran los métodos de la lista de usuarios
    public interface UsuarioRepository
    {
        ICollection<User> GetUsuarios();
        User GetUsuario(string id);
        User ActualizarUsuario(User usuario);
    }
}
