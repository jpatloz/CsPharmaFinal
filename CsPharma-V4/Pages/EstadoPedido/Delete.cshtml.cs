﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CsPharma_V4.Pages.EstadoPedido
{
    [Authorize(Roles = "Administradores, Empleados")]
    public class DeleteModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public DeleteModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        [BindProperty]
      public TdcTchEstadoPedido TdcTchEstadoPedido { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                return NotFound();
            }

            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FirstOrDefaultAsync(m => m.Id == id);

            if (tdctchestadopedido == null)
            {
                return NotFound();
            }
            else 
            {
                TdcTchEstadoPedido = tdctchestadopedido;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TdcTchEstadoPedidos == null)
            {
                return NotFound();
            }
            var tdctchestadopedido = await _context.TdcTchEstadoPedidos.FindAsync(id);

            if (tdctchestadopedido != null)
            {
                TdcTchEstadoPedido = tdctchestadopedido;
                _context.TdcTchEstadoPedidos.Remove(TdcTchEstadoPedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
