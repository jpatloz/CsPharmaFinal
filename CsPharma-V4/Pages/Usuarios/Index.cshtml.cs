using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsPharma_V4.Pages.Usuarios
{
    [Authorize(Roles = "Administradores")]
    public class IndexModel : PageModel
    {
        private readonly LoginContexto _context;

        public IndexModel(LoginContexto context)
        {
            _context = context;
        }

        public IList<User> listaUsuarios { get; set; } = default!;

        public async Task<PageResult> OnGetAsync()
        {
            if (_context.UserSet != null)
            {
                listaUsuarios = await _context.UserSet.ToListAsync();
            }
            return Page();
        }
    }
}
