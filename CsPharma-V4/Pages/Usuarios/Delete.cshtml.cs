using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsPharma_V4.Pages.Usuarios
{
    //Autoriza el acceso al controlador 
    [Authorize(Roles = "Administradores")]
    public class DeleteModel : PageModel
    {
        private readonly LoginContexto _context;
        
    public DeleteModel(LoginContexto context)
        {
            _context = context;
        }

        [BindProperty]
        public User Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(id == null || _context.UserSet == null)
            {
                return NotFound();
            }

            var usuario = await _context.UserSet.FirstOrDefaultAsync(m => m.Id == id);

            if(usuario == null)
            {
                return NotFound();
            }
            else
            {
                Usuario = usuario;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(id == null || _context.UserSet == null)
            {
                return NotFound();
            }

            var usuario = await _context.UserSet.FindAsync(id);

            if(usuario != null)
            {
                Usuario = usuario;
                _context.UserSet.Remove(Usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
