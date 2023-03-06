using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
{

    //Autoriza el acceso al controlador 
    [Authorize(Roles = "Administradores, Empleados")]
    public class EditModel : PageModel
    {
        private readonly CsPharmaV4Context _context;

        // Se define el constructor de la clase, el cual recibe un objeto de tipo CsPharmaV4Context y lo asigna a la variable declarada anteriormente
        public EditModel(CsPharmaV4Context context)
        {
            _context = context;
        }

        // Se define una propiedad BindProperty, la cual se utilizará para enlazar los datos del modelo con la vista
        [BindProperty]
        public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; } = default!;

        // Se define el método OnGetAsync, que se ejecutará cuando se realice una petición HTTP GET a esta página
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Si el parámetro id es nulo o la tabla TdcTchEstadoPedidos está vacía, se retorna un error 404
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                return NotFound();
            }

            // Se obtiene el primer registro de la tabla TdcTchEstadoPedidos que coincida con el id proporcionado
            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FirstOrDefaultAsync(m => m.Id == id);
            // Si el registro no se encuentra, se retorna un error 404
            if (tdctchestadopedido == null)
            {
                return NotFound();
            }
            // Se asigna el registro encontrado a la propiedad TdcTchEstadoPedido, la cual está enlazada con la vista
            TdcTchEstadoPedido = tdctchestadopedido;
            // Se agregan los datos necesarios a la ViewData para poder mostrarlos en la vista
            ViewData["CodEstadoDevolucion"] = new SelectList(_context.TdcCatEstadosDevolucionPedidos, "CodEstadoDevolucion", "DesEstadoDevolucion");
            ViewData["CodEstadoEnvio"] = new SelectList(_context.TdcCatEstadosEnvioPedidos, "CodEstadoEnvio", "DesEstadoEnvio");
            ViewData["CodEstadoPago"] = new SelectList(_context.TdcCatEstadosPagoPedidos, "CodEstadoPago", "DesEstadoPago");
            ViewData["CodLinea"] = new SelectList(_context.TdcCatLineasDistribucions, "CodLinea", "CodLinea");
            // Se retorna la vista correspondiente
            return Page();
        }

        // Se define el método OnPostAsync, que se ejecutará cuando se realice una petición HTTP POST a esta página
        public async Task<IActionResult> OnPostAsync()
        {
            // Se actualiza el estado del objeto TdcTchEstadoPedido para que se pueda actualizar en la base de datos
            _context.Attach(TdcTchEstadoPedido).State = EntityState.Modified;

            try
            {
                // Se guardan los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si se produce un error de concurrencia al guardar los cambios, se verifica si el registro aún existe
                if (!TdcTchEstadoPedidoExists(TdcTchEstadoPedido.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //Redirección al index
            return RedirectToPage("./Index");
        }

        //Método ue devuelve un booleano en caso de que exista el pedido
        private bool TdcTchEstadoPedidoExists(int id)
        {
          return _context.TdcTchEstadoPedidos.Any(e => e.Id == id);
        }
    }
}
