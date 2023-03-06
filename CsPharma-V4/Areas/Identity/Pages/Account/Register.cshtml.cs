// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        // Inicializar los campos privados necesarios para la gestión de registro
        private readonly SignInManager<User> _signInManager; // Maneja la lógica de inicio de sesión de usuario
        private readonly UserManager<User> _userManager; // Maneja la lógica de autenticación de usuario
        private readonly RoleManager<IdentityRole> _roleManager; // Maneja la lógica de roles de usuario
        private readonly IUserStore<User> _userStore; // Accede al almacenamiento de usuarios en la aplicación
        private readonly IUserEmailStore<User> _emailStore; // Accede al almacenamiento de correos electrónicos de usuarios en la aplicación
        private readonly ILogger<RegisterModel> _logger; // Registra y maneja mensajes de registro para esta clase
        private readonly IEmailSender _emailSender; // Envía correos electrónicos a los usuarios registrados

        // Constructor que inicializa los campos privados con las instancias necesarias
        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // Propiedad para enlazar datos de entrada del usuario con la vista
        [BindProperty]
        public InputModel Input { get; set; }

        // Propiedad para almacenar la URL a la que se redirigirá después de que se complete el registro del usuario
        public string ReturnUrl { get; set; }

        // Propiedad para almacenar la lista de proveedores de autenticación externos
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Clase para definir los datos de entrada del usuario para el registro
        public class InputModel
        {
            // Campo para el nombre del usuario
            [Required]
            [StringLength(255, ErrorMessage = "El campo del nombre tiene un máximo de 255 caracteres")]
            [Display(Name = "Nombre")]
            public string NombreUsuario { get; set; }

            // Campo para el apellido del usuario
            [Required]
            [StringLength(255, ErrorMessage = "El campo de apellidos tiene un máximo de 255 caracteres")]
            [Display(Name = "Apellidos")]
            public string ApellidosUsuario { get; set; }

            // Campo para el correo electrónico del usuario
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            // Campo para la contraseña del usuario
            [Required]
            [StringLength(100, ErrorMessage = "No alcanza el mínimo de carácteres requeridos", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Contraseña { get; set; }

            // Campo para confirmar la contraseña del usuario
            [DataType(DataType.Password)]
            [Display(Name = "ConfirmarContraseña")]
            [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmarContraseña { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            // Obtiene la lista de esquemas de autenticación externos y la convierte en una lista
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Establece la URL de retorno si no se especifica ninguna
            returnUrl ??= Url.Content("~/");

            // Obtiene la lista de esquemas de autenticación externos y la convierte en una lista
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Crea un nuevo usuario y establece su información de nombre de usuario y apellidos
                var user = CreateUser();
                user.NombreUsuario = Input.NombreUsuario;
                user.ApellidosUsuario = Input.ApellidosUsuario;

                // Establece el nombre de usuario y correo electrónico del usuario
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Crea el usuario y establece la contraseña
                var result = await _userManager.CreateAsync(user, Input.Contraseña);

                if (result.Succeeded)
                {
                    // Registra la creación del usuario en el registro
                    _logger.LogInformation("Usuario creado");

                    // Verifica si existe un rol llamado "Usuarios"
                    var role = await _roleManager.RoleExistsAsync("Usuarios");

                    // Si no existe el rol, lo crea
                    if (!role)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Usuarios"));
                    }

                    // Agrega al usuario al rol de "Usuarios"
                    await _userManager.AddToRoleAsync(user, "Usuarios");

                    // Genera el código de confirmación de correo electrónico y la URL de callback para confirmar la dirección de correo electrónico
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // Envía el correo electrónico de confirmación
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // Si la confirmación de correo electrónico es obligatoria, redirige al usuario a la página de confirmación
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    // Si no, inicia sesión al usuario y lo redirige a la página de inicio
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // Si la creación del usuario no tuvo éxito, agrega los errores al ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // Retorna la página actual para mostrar el mensaje de error
            return Page();
        }

        // Método privado que instancia un nuevo objeto User
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                // Lanza una excepción si no se puede crear una instancia de User
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        // Método privado que devuelve una instancia de IUserEmailStore<User>
        private IUserEmailStore<User> GetEmailStore()
        {
            // Lanza una excepción si el usuario no admite email
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
