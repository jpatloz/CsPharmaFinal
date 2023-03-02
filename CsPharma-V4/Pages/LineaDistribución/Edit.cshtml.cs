﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace CsPharma_V4.Pages.LineaDistribución
{
    public class EditModel : PageModel
    {
        private readonly DAL.Models.CsPharmaV4Context _context;

        public EditModel(DAL.Models.CsPharmaV4Context context)
        {
            _context = context;
        }

        [BindProperty]
        public TdcCatLineasDistribucion TdcCatLineasDistribucion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.TdcCatLineasDistribucions == null)
            {
                return NotFound();
            }

            var tdccatlineasdistribucion =  await _context.TdcCatLineasDistribucions.FirstOrDefaultAsync(m => m.CodLinea == id);
            if (tdccatlineasdistribucion == null)
            {
                return NotFound();
            }
            TdcCatLineasDistribucion = tdccatlineasdistribucion;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TdcCatLineasDistribucion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TdcCatLineasDistribucionExists(TdcCatLineasDistribucion.CodLinea))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TdcCatLineasDistribucionExists(string id)
        {
          return _context.TdcCatLineasDistribucions.Any(e => e.CodLinea == id);
        }
    }
}
