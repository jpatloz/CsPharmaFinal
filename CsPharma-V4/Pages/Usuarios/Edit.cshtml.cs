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
    [Authorize(Roles = "Administradores")]
    public class EditModel : PageModel
    {

        private readonly WorkRepository _workRepository;
        private SignInManager<User> _signInManager;

        public EditModel(WorkRepository workRepository, SignInManager<User> signInManager)
        {
            _workRepository = workRepository;
            _signInManager = signInManager;
        }

        [BindProperty]
        public ModeloUsuario ModeloUsuario { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var usuario = _workRepository.UsuariosRepo.GetUsuario(id);
            var rol = _workRepository.RolesRepo.GetRoles();

            if (usuario == null || rol == null)
            {
                return NotFound();
            }

            var rolesUsuario = await _signInManager.UserManager.GetRolesAsync(usuario);

            var rolesItems = rol.Select(roles =>
                new SelectListItem(
                    roles.Name,
                    roles.Id,
                    rolesUsuario.Any(u => u.Contains(roles.Name))
                )).ToList();

            var modeloUsuario = new ModeloUsuario
            {
                Usuarios = usuario,
                Roles = rolesItems
            };

            ModeloUsuario = modeloUsuario;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ModeloUsuario modeloUsuario)
        {
            if (modeloUsuario == null)
            {
                return NotFound();
            }

            var usuario = _workRepository.UsuariosRepo.GetUsuario(modeloUsuario.Usuarios.Id);

            if (usuario == null)
            {
                return NotFound();
            }

            var rolesUsuarioDB = await _signInManager.UserManager.GetRolesAsync(usuario);

            var añadirRol = new List<string>();
            var eliminarRol = new List<string>();

            if (modeloUsuario.Roles != null)
            {
                foreach (var rol in modeloUsuario.Roles)
                {
                    var rolDB = rolesUsuarioDB.FirstOrDefault(u => u == rol.Text);

                    if (rol.Selected)
                    {
                        if (rolDB == null)
                        {
                            añadirRol.Add(rol.Text);
                        }
                    }
                    else
                    {
                        if (rolDB != null)
                        {
                            eliminarRol.Add(rol.Text);
                        }
                    }
                }
            }

            if (añadirRol.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(usuario, añadirRol);
            }
            else if (eliminarRol.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(usuario, eliminarRol);
            }

            usuario.UserName = modeloUsuario.Usuarios.UserName;
            usuario.Email = modeloUsuario.Usuarios.Email;
            usuario.PhoneNumber = modeloUsuario.Usuarios.PhoneNumber;

            _workRepository.UsuariosRepo.ActualizarUsuario(usuario);

            return RedirectToPage("./Index");
        }
    }
}