using ForLifeWeb.Data;
using ForLifeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ForLifeWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var find = Usuario.ValidarLogin(usuario.cod_usuario, usuario.senha);

            if (find)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.cod_usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (Url.IsLocalUrl(url: "/teste"))
                {
                    return Redirect(url: "/teste");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login invalido");
            }

            return View();

        }
        
    }
}
