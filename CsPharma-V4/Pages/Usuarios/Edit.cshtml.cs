using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsPharma_V4.Pages.Usuarios
{
    public class EditModel : PageModel
    {

        private readonly WorkRepository _workRepository;

        public EditModel(WorkRepository workRepository)
        {
            _workRepository = workRepository;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = _workRepository.Usuarios.GetUser(id);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            

            return RedirectToPage("./Index");
        }

    }
}
