using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CsPharma_V4.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        // Constructor de la clase
        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Método que se ejecuta al enviar el formulario de cerrar sesión
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Cierra la sesión del usuario actual
            await _signInManager.SignOutAsync();

            // Registra un mensaje de información en el log
            _logger.LogInformation("Sesión de usuario cerrada");

            // Redirige al usuario al URL especificado en returnUrl, si existe
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            // De lo contrario, redirige al usuario a la página de inicio
            else
            {
                return RedirectToPage();
            }
        }
    }
}
