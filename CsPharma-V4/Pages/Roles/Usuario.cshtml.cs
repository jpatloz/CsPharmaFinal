using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.Roles
{

     [Authorize(Roles = "Usuarios")]
    public class UsuarioModel : PageModel
    {
        public IActionResult Index()
        {
            return Page();
        }
    }
}
