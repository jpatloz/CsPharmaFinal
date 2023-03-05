using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CsPharma_V4.Modelo
{
    public class ModeloUsuario
    {
        public User Usuarios { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}
