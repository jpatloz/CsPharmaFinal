namespace CsPharma_V4.Core.Repositorios
{
    //Interfaz donde llamamos a los repositorios de rol y usuario
    public interface WorkRepository
    {
        UsuarioRepository UsuariosRepo { get; }
        RolRepository RolesRepo { get; }
    }
}
