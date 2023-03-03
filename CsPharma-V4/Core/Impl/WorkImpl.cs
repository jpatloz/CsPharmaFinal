using CsPharma_V4.Core.Repositorios;

namespace CsPharma_V4.Core.Impl
{
    public class WorkImpl : WorkRepository
    {
        public UsuarioRepository Usuarios { get; }

        public WorkImpl(UsuarioRepository usuarios)
        {
            Usuarios=usuarios;
        }
    }
}
