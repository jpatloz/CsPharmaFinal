using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsPharma_V4.Pages.Usuarios
{
    [Authorize(Roles = "Administradores")]
    public class IndexModel : PageModel
    {
        // Atributo para llamar al contexto de la base de datos
        private readonly LoginContexto _context;

        // Constructor que recibe el contexto de la base de datos como parámetro y lo almacena en el atributo
        public IndexModel(LoginContexto context)
        {
            _context = context;
        }

        public IList<User> listaUsuarios { get; set; } = default!;
        public string Filtro { get; set; }

        public async Task<PageResult> OnGetAsync(string filtro)
        {
            IQueryable<User> query = _context.UserSet;

            //Si el contexto de usuario no es nulo, obtiene la lista completa de usuarios
            if (_context.UserSet != null)
            {
                listaUsuarios = await _context.UserSet.ToListAsync();
            }

            //Método para que el filtro busque por el nombre de usuario, email y teléfono
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(u => u.UserName.Contains(filtro) || u.Email.Contains(filtro) || u.PhoneNumber.Contains(filtro));
            }

            //Ejecuta la consulta de la lista de usuarios
            listaUsuarios = await query.ToListAsync();
            Filtro = filtro;

            //Devuelve la página
            return Page();
        }
    }
}
