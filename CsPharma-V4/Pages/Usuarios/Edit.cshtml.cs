using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using CsPharma_V4.Modelo;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace CsPharma_V4.Pages.Usuarios
{
    // Significa que solo los usuarios que tengan el rol "Administradores" pueden acceder a esta p�gina.
    [Authorize(Roles = "Administradores")]
    public class EditModel : PageModel
    {

        private readonly WorkRepository _workRepository;
        private SignInManager<User> _signInManager;

        // Constructor de la clase EditModel
        public EditModel(WorkRepository workRepository, SignInManager<User> signInManager)
        {
            _workRepository = workRepository;
            _signInManager = signInManager;
        }

        // Propiedad BindProperty de la clase ModeloUsuario
        [BindProperty]
        public ModeloUsuario ModeloUsuario { get; set; } = default!;

        // M�todo que se ejecuta al acceder a la p�gina de edici�n de usuarios
        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Obtiene el usuario con el id especificado
            var usuario = _workRepository.UsuariosRepo.GetUsuario(id);
            // Obtiene todos los roles existentes
            var roles = _workRepository.RolesRepo.GetRoles();
            // Obtiene los roles del usuario
            var rolesUsuario = await _signInManager.UserManager.GetRolesAsync(usuario);

            // Si el usuario no existe, devuelve una respuesta 404
            if (usuario == null)
            {
                return NotFound();
            }

            // Crea una lista de SelectListItem con los roles del usuario
            var rolesItems = roles.Select(rol =>
                new SelectListItem(
                    rol.Name,
                    rol.Id,
                    rolesUsuario.Any(u => u.Contains(rol.Name))
                )).ToList();

            // Crea un objeto de ModeloUsuario que incluye el usuario y sus roles
            var modeloUsuario = new ModeloUsuario
            {
                Usuarios = usuario,
                Roles = rolesItems
            };

            // Asigna el objeto modeloUsuario a la propiedad ModeloUsuario
            ModeloUsuario = modeloUsuario;

            // Devuelve la p�gina
            return Page();
        }

        // M�todo que se ejecuta al enviar el formulario de edici�n de usuarios
        public async Task<IActionResult> OnPostAsync(ModeloUsuario modeloUsuario)
        {
            // Obtiene el usuario con el id especificado
            var usuario = _workRepository.UsuariosRepo.GetUsuario(modeloUsuario.Usuarios.Id);

            // Si el usuario no existe, devuelve una respuesta 404
            if (usuario == null)
            {
                return NotFound();
            }

            // Obtiene los roles del usuario de la base de datos
            var rolesUsuarioDB = await _signInManager.UserManager.GetRolesAsync(usuario);

            // Crea dos listas vac�as, una para los roles que se van a a�adir y otra para los que se van a eliminar
            var a�adirRol = new List<string>();
            var eliminarRol = new List<string>();

            // Recorre los roles seleccionados en el formulario
            foreach (var rol in modeloUsuario.Roles)
            {
                // Busca el rol en la lista de roles del usuario en la base de datos
                var rolDB = rolesUsuarioDB.FirstOrDefault(u => u == rol.Text);

                // Si el rol est� seleccionado en el formulario y no est� en la lista de roles del usuario en la base de datos, lo a�ade a la lista de roles a a�adir
                if (rol.Selected)
                {
                    if (rolDB == null)
                    {
                        a�adirRol.Add(rol.Text);
                    }
                }
                // Si el rol no est� seleccionado en el formulario y est� en la lista de roles del usuario en la base de datos, lo a�ade a la lista de roles a eliminar
                else
                {
                    if (rolDB != null)
                    {
                        eliminarRol.Add(rol.Text);
                        }
                    }
                }

            // Se comprueba si la lista de roles a a�adir contiene alg�n elemento para a�adirlo

            if (a�adirRol.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(usuario, a�adirRol);
            }

            // Se comprueba si la lista de roles a eliminar contiene alg�n elemento para eliminarlo

            if (eliminarRol.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(usuario, eliminarRol);
            }

            //Se actualizan los campos con el valor que se encuentra dentro de modelousuario
            usuario.UserName = modeloUsuario.Usuarios.UserName;
            usuario.Email = modeloUsuario.Usuarios.Email;
            usuario.PhoneNumber = modeloUsuario.Usuarios.PhoneNumber;

            //Se actualiza al usuario
            _workRepository.UsuariosRepo.ActualizarUsuario(usuario);

            //Se redirige al index
            return RedirectToPage("./Index");
        }
    }
}