using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace CsPharma_V4.Pages.Roles
{
    public class EmpleadosModel : PageModel
    {


        [Authorize(Roles = "Empleados")]


        public IActionResult Index()
            {
                return Page();
            }
        }
    }

