using ForLifeWeb.Data;
using ForLifeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ForLifeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Menu", "Home"); 
            }

            return View();
        }

        [Authorize]
        public IActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string usuario, string senha, bool lembrarMe)
        {

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                ViewBag.ErrorMessage = "Usu�rio e senha s�o obrigat�rios.";
                return View("Index");
            }

            // Valida o usu�rio com LINQ
            var usuarioValido = _context.Usuarios
                .FirstOrDefault(u => u.cod_usuario == usuario && u.senha== senha);

            if (usuarioValido != null)
            {
                // Cria��o de claims para o usu�rio
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Faz o login do usu�rio (sign in)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Se o "lembrar-me" for selecionado, cria o cookie persistente
                if (lembrarMe)
                {
                    Response.Cookies.Append("UserId", usuarioValido.id_usuario.ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7) // Cookie persistente por 7 dias
                    });
                }
                else
                {
                    // Caso contr�rio, cria o cookie de sess�o
                    Response.Cookies.Append("UserId", usuarioValido.id_usuario.ToString());
                }

                return RedirectToAction("Menu", "Home"); // Redireciona para o Menu
            }

            // Caso de falha no login
            ViewBag.ErrorMessage = "Usu�rio ou senha inv�lidos.";
            return View("Index"); // Recarrega a tela de login
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Remove o cookie de autentica��o
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove o cookie persistente
            Response.Cookies.Delete("UserId");

            return RedirectToAction("Index", "Home"); // Redireciona para a p�gina de login
        }
    }
}
