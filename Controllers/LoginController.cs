using ForLifeWeb.Data;
using ForLifeWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
        [ValidateAntiForgeryToken]
        public IActionResult Validar(string codUsuario, string senha)
        {
            // Busca o usuário pelo código
            var usuario = _context.Usuarios.FirstOrDefault(u => u.cod_usuario == codUsuario);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuário não encontrado.");
                return View("Index");
            }

            if (usuario.senha != senha)
            {
                ModelState.AddModelError("", "Senha incorreta.");
                return View("Index");
            }

            // Login bem-sucedido
            TempData["Mensagem"] = "Login realizado com sucesso!";
            return RedirectToAction("Index", "Home");
        }
    }
}
