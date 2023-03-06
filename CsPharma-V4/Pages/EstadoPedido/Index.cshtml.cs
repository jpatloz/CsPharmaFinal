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
        private readonly CsPharmaV4Context _context;

        public IndexModel(CsPharmaV4Context context)
        {
            _context = context;
        }

        public IList<TdcTchEstadoPedido> TdcTchEstadoPedido { get;set; } = default!;
        public string Filtro { get; set; }

        public async Task OnGetAsync(string filtro)
        {
                IQueryable<TdcTchEstadoPedido> query = _context.TdcTchEstadoPedidos
                    .Include(t => t.CodEstadoDevolucionNavigation)
                    .Include(t => t.CodEstadoEnvioNavigation)
                    .Include(t => t.CodEstadoPagoNavigation)
                    .Include(t => t.CodLineaNavigation);

                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(t => t.CodEstadoDevolucionNavigation.DesEstadoDevolucion.Contains(filtro)
                        || t.CodEstadoEnvioNavigation.DesEstadoEnvio.Contains(filtro)
                        || t.CodEstadoPagoNavigation.DesEstadoPago.Contains(filtro)
                        || t.CodLinea.Contains(filtro) || t.CodPedido.Contains(filtro));
                }

                TdcTchEstadoPedido = await query.ToListAsync();
                Filtro = filtro;
            }
        }
    }

