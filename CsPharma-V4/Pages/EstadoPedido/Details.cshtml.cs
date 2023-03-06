using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace CsPharma_V4.Pages.EstadoPedido
{
    public class DetailsModel : PageModel
    {
        // Se declara una instancia del contexto de base de datos "CsPharmaV4Context"
        private readonly CsPharmaV4Context _context;

        // Se define el constructor que recibe una instancia del contexto de base de datos "CsPharmaV4Context"
        public DetailsModel(CsPharmaV4Context context)
        {
            _context = context;
        }

        // Se declara la propiedad "TdcTchEstadoPedido" que representa el modelo de datos que se mostrará en la vista
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }

        // Se define el método "OnGetAsync" que se llama cuando se envía una solicitud GET a la página
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Se verifica si el valor del parámetro "id" es nulo o si la tabla "TdcTchEstadoPedidos" es nula
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                // Si es así, se devuelve una respuesta "NotFound" indicando que no se ha encontrado el recurso solicitado
                return NotFound();
            }


            TdcTchEstadoPedido = await _context.TdcTchEstadoPedidos
               .Include(t => t.CodEstadoDevolucionNavigation)
               .Include(t => t.CodEstadoEnvioNavigation)
               .Include(t => t.CodEstadoPagoNavigation)
               .Include(t => t.CodLineaNavigation)
               .FirstOrDefaultAsync(m => m.Id == id);

            // Se busca el registro en la tabla "TdcTchEstadoPedidos" utilizando el valor del parámetro "id"
            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FirstOrDefaultAsync(m => m.Id == id);

            // Si no se encuentra el registro, se devuelve una respuesta "NotFound"
            if (tdctchestadopedido == null)
            {
                return NotFound();
            }
            else
            {
                // Si se encuentra el registro, se asigna a la propiedad "TdcTchEstadoPedido"
                TdcTchEstadoPedido = tdctchestadopedido;
            }

            // Se devuelve una respuesta "Page" que renderiza la vista asociada al controlador
            return Page();
        }
    }
}
