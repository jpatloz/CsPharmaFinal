using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CsPharma_V4.Modelo
{
    public class ModeloUsuario
    {
        // Propiedad que representa un objeto User
        public User Usuarios { get; set; }

        // Propiedad que representa una lista de objetos SelectListItem
        public List<SelectListItem> Roles { get; set; }
    }
}
