using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForLifeWeb.Data;
using ForLifeWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace ForLifeWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuario
        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.Usuarios.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.cpf.Contains(searchTerm) ||
                    x.cargo.Contains(searchTerm) ||
                    x.cod_usuario.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td><input type='checkbox' class='select-usuario' value='{item.id_usuario}'></td>
                        <td>{item.id_usuario}</td>
                        <td>{item.nome}</td>
                        <td>{item.cpf}</td>
                        <td>{item.cod_usuario}</td>
                        <td>{item.cargo}</td>
                    </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        // GET: Usuario/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.id_usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_usuario,nome,cargo,cpf,cod_usuario,senha,data_cadastro,ativo")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {  
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Isso vai mostrar os erros de validação no console ou log
                    Console.WriteLine(error.ErrorMessage);
                }
                //return View(usuario);
            }
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_usuario,nome,cargo,cpf,cod_usuario,senha,data_cadastro")] Usuario usuario)
        {
            if (id != usuario.id_usuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.id_usuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.id_usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.id_usuario == id);
        }


        private string FormatCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return cpf;

            var cleanCpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cleanCpf.Length == 11)
            {
                return string.Format("{0}.{1}.{2}-{3}",
                    cleanCpf.Substring(0, 3),
                    cleanCpf.Substring(3, 3),
                    cleanCpf.Substring(6, 3),
                    cleanCpf.Substring(9, 2));
            }

            return cpf;
        }


    }
}
