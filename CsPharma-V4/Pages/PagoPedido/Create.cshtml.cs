using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.PagoPedido
{
    [Authorize(Roles = "Administradores, Empleados")]
    public class CreateModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public CreateModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TdcCatEstadosPagoPedido TdcCatEstadosPagoPedido { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TdcCatEstadosPagoPedidos.Add(TdcCatEstadosPagoPedido);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
