using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsPharma_V4.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly LoginContexto _context;

        private readonly WorkRepository _workRepository;
        public IndexModel(LoginContexto context, WorkRepository workRepository)
        {
            _context = context;
            _workRepository = workRepository;
        }

        public IList<User> User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Users != null)
            {
                User = await _context.Users.ToListAsync();
            }
            return Page();
        }
    }
}
