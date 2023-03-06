using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
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
        ViewData["CodEstadoDevolucion"] = new SelectList(_context.TdcCatEstadosDevolucionPedidos, "CodEstadoDevolucion", "DesEstadoDevolucion");
        ViewData["CodEstadoEnvio"] = new SelectList(_context.TdcCatEstadosEnvioPedidos, "CodEstadoEnvio", "DesEstadoEnvio");
        ViewData["CodEstadoPago"] = new SelectList(_context.TdcCatEstadosPagoPedidos, "CodEstadoPago", "DesEstadoPago");
        ViewData["CodLinea"] = new SelectList(_context.TdcCatLineasDistribucions, "CodLinea", "CodLinea");
            return Page();
        }

        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            TdcTchEstadoPedido.MdUuid = Guid.NewGuid().ToString();
            TdcTchEstadoPedido.MdDate = DateTime.Now;
            _context.TdcTchEstadoPedidos.Add(TdcTchEstadoPedido);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
