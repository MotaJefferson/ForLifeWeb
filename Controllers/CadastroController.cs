using ForLifeWeb.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace ForLifeWeb.Controllers
{

    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Cadastro
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }
    }
}
