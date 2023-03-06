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
    // Se autoriza el acceso al controlador para los roles "Administradores" y "Empleados" 
    [Authorize(Roles = "Administradores, Empleados")]
    public class CreateModel : PageModel
    {
        // Se declara una instancia del contexto de base de datos "CsPharmaV4Context"
        private readonly CsPharmaV4Context _context;

        // Se define el constructor que recibe una instancia del contexto de base de datos "CsPharmaV4Context"
        public CreateModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        // Se ejecuta al cargar la página de creación y se encarga de pasar los datos necesarios a la vista
        public IActionResult OnGet()
        {
            ViewData["CodEstadoDevolucion"] = new SelectList(_context.TdcCatEstadosDevolucionPedidos, "CodEstadoDevolucion", "DesEstadoDevolucion");
            ViewData["CodEstadoEnvio"] = new SelectList(_context.TdcCatEstadosEnvioPedidos, "CodEstadoEnvio", "DesEstadoEnvio");
            ViewData["CodEstadoPago"] = new SelectList(_context.TdcCatEstadosPagoPedidos, "CodEstadoPago", "DesEstadoPago");
            ViewData["CodLinea"] = new SelectList(_context.TdcCatLineasDistribucions, "CodLinea", "CodLinea");
            return Page();
        }

        // Se utiliza para enlazar los valores de la vista con la propiedad TdcTchEstadoPedido del modelo
        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }

        // Se ejecuta al hacer clic en el botón de envío del formulario de creación
        public async Task<IActionResult> OnPostAsync()
        {
            // Se generan los valores de MdUuid y MdDate
            TdcTchEstadoPedido.MdUuid = Guid.NewGuid().ToString();
            TdcTchEstadoPedido.MdDate = DateTime.Now;

            // Se añade el objeto TdcTchEstadoPedido a la base de datos y se guardan los cambios
            _context.TdcTchEstadoPedidos.Add(TdcTchEstadoPedido);
            await _context.SaveChangesAsync();

            // Se redirige a la página de índice después de agregar el nuevo registro
            return RedirectToPage("./Index");
        }
    }
}
