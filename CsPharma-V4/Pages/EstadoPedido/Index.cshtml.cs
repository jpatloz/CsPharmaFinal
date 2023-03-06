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
    public class IndexModel : PageModel
    {
        // Atributo para llamar al contexto de la base de datos
        private readonly CsPharmaV4Context _context;

        // Constructor que recibe el contexto de la base de datos como parámetro y lo almacena en el atributo
        public IndexModel(CsPharmaV4Context context)
        {
            _context = context;
        }

        // Atributos del modelo
        public IList<TdcTchEstadoPedido> TdcTchEstadoPedido { get; set; } = default!;
        public string Filtro { get; set; }

        // Método que se ejecuta al cargar la página, recibe un parámetro filtro que se utilizará para buscar en la base de datos
        public async Task OnGetAsync(string filtro)
        {
            // Se crea una consulta que incluye todos los datos necesarios para mostrar los estados de pedidos
            IQueryable<TdcTchEstadoPedido> query = _context.TdcTchEstadoPedidos
                .Include(t => t.CodEstadoDevolucionNavigation)
                .Include(t => t.CodEstadoEnvioNavigation)
                .Include(t => t.CodEstadoPagoNavigation)
                .Include(t => t.CodLineaNavigation);

            // Si el parámetro filtro no es nulo ni vacío, se agregan condiciones a la consulta para buscar en los campos indicados
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(t => t.CodEstadoDevolucionNavigation.DesEstadoDevolucion.Contains(filtro)
                    || t.CodEstadoEnvioNavigation.DesEstadoEnvio.Contains(filtro)
                    || t.CodEstadoPagoNavigation.DesEstadoPago.Contains(filtro)
                    || t.CodLinea.Contains(filtro) || t.CodPedido.Contains(filtro));
            }

            // Se ejecuta la consulta y se almacena en la propiedad TdcTchEstadoPedido para ser utilizada en la vista
            TdcTchEstadoPedido = await query.ToListAsync();

            // Se almacena el filtro utilizado para que pueda ser utilizado en la vista
            Filtro = filtro;
        }
    }
    }

