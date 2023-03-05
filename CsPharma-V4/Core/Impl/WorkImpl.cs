using CsPharma_V4.Core.Repositorios;


//Clase que implementa la interfaz work

namespace CsPharma_V4.Core.Impl
{
    public class WorkImpl : WorkRepository
    {
        public UsuarioRepository UsuariosRepo { get; }
        public RolRepository RolesRepo { get; }

        public WorkImpl(UsuarioRepository usuariosRepo, RolRepository rolesRepo)
        {
            UsuariosRepo = usuariosRepo;
            RolesRepo = rolesRepo;
        }
    }
}
