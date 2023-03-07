
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        // Constructor de la clase LoginModel que recibe el SignInManager y el ILogger como dependencias
        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Propiedad BindProperty para poder recibir los datos del formulario de inicio de sesión
        [BindProperty]
        public InputModel Input { get; set; }

        // Lista de los esquemas de autenticación externos disponibles
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // URL a la que se redirigirá después del inicio de sesión
        public string ReturnUrl { get; set; }

        // Propiedad TempData para mostrar mensajes de error en la vista
        [TempData]
        public string ErrorMessage { get; set; }

        // Clase interna que representa los campos del formulario de inicio de sesión
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Recuérdame")]
            public bool RememberMe { get; set; }
        }

        // Método OnGetAsync que se llama cuando se carga la página de inicio de sesión
        public async Task OnGetAsync(string returnUrl = null)
        {
            // Si hay algún mensaje de error, se agrega al modelo de la página
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Si no se especificó una URL de retorno, se redirige a la página principal
            returnUrl ??= Url.Content("~/");

            // Cierra cualquier sesión externa que pueda existir
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Obtiene los esquemas de autenticación externos disponibles
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Guarda la URL de retorno en una propiedad para usarla en el OnPostAsync
            ReturnUrl = returnUrl;
        }

        // Método OnPostAsync que se llama cuando se envía el formulario de inicio de sesión
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Si no se especificó una URL de retorno, se redirige a la página principal
            returnUrl ??= Url.Content("~/");

            // Obtiene los esquemas de autenticación externos disponibles
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Comprobar si las credenciales de inicio de sesión son válidas
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                // Si las credenciales son válidas, redirigir al usuario a la página solicitada
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuario logueado.");
                    return LocalRedirect(returnUrl);
                }

                // Si se requiere autenticación de dos factores, redirigir al usuario a la página de inicio de sesión con 2FA
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                // Si la cuenta está bloqueada, redirigir al usuario a la página de bloqueo de cuenta
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // Si las credenciales no son válidas, agregar un error de modelo y volver a mostrar la página de inicio de sesión
                    ModelState.AddModelError(string.Empty, "Error de inicio de sesión.");
                    return Page();
                }
            }

            // Si el modelo no es válido, volver a mostrar la página de inicio de sesión
            return Page();
        }
    }
}
