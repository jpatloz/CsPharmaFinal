using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using Microsoft.AspNetCore.Identity;


//Clase que implementa la interfaz de los roles
//Identity Role es la clase que nos ayudará a gestionar la administración de roles y permisos de identity

namespace CsPharma_V4.Core.Impl
{
    public class RolImpl: RolRepository
    {
        private readonly LoginContexto _context;

        public RolImpl(LoginContexto loginContexto)
        {
            _context = loginContexto;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }

    }
}
