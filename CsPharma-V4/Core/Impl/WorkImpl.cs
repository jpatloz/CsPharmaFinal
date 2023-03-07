using CsPharma_V4.Core.Repositorios;


//Clase que implementa la interfaz work

namespace CsPharma_V4.Core.Impl
{
    public class WorkImpl : WorkRepository
    {
        // Propiedad que permite acceder al repositorio de usuarios
        public UsuarioRepository UsuariosRepo { get; }

        // Propiedad que permite acceder al repositorio de roles
        public RolRepository RolesRepo { get; }

        // Constructor que recibe instancias de los repositorios de usuarios y roles
        public WorkImpl(UsuarioRepository usuariosRepo, RolRepository rolesRepo)
        {
            // Se asignan las instancias recibidas a las propiedades correspondientes
            UsuariosRepo = usuariosRepo;
            RolesRepo = rolesRepo;
        }
    }
}
