using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using Microsoft.AspNetCore.Identity;


//Clase que implementa la interfaz de los roles
//Identity Role es la clase que nos ayudará a gestionar la administración de roles y permisos de identity

namespace CsPharma_V4.Core.Impl
{
        // Esta clase implementa la interfaz RolRepository
        public class RolImpl : RolRepository
        {
            // Esta variable guarda una instancia del contexto de Login
            private readonly LoginContexto _context;

            // Constructor que recibe una instancia de LoginContexto
            public RolImpl(LoginContexto loginContexto)
            {
                _context = loginContexto;
            }

            // Método que devuelve una colección de objetos IdentityRole
            public ICollection<IdentityRole> GetRoles()
            {
                // Se obtienen todos los roles del contexto de Login y se convierten en una lista
                return _context.Roles.ToList();
            }
        }
    }
