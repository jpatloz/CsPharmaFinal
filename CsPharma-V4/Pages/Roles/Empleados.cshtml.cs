using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace CsPharma_V4.Pages.Roles
{
    [Authorize(Roles = "Administradores, Empleados")]
    public class EmpleadosModel : PageModel
    {
        
        public IActionResult Index()
            {
                return Page();
            }
        }
    }

