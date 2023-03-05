using Microsoft.AspNetCore.Identity;

namespace CsPharma_V4.Core.Repositorios
{

    //Interfaz donde se define el método para coger el rol del usuario
    //Identity Role es la clase que nos ayudará a gestionar la administración de roles y permisos de identity

    public interface RolRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
 }
