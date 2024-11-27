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
    public class FornecedorController : Controller
    {
        private readonly AppDbContext _context;

        public FornecedorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Fornecedor
        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.Fornecedores.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.razao_social.Contains(searchTerm) ||
                    x.cpf.Contains(searchTerm) ||
                    x.cnpj.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if(Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td><input type='checkbox' class='select-cliente' value='{item.id_fornecedor}'></td>
                        <td>{item.id_fornecedor}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nome) ? item.razao_social : item.nome)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.cpf) ? item.cnpj : item.cpf)}</td>
                        <td>{item.telefone}</td>
                        <td>{item.observacoes}</td>
                    </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        // GET: Fornecedor/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.id_fornecedor == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // GET: Fornecedor/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_fornecedor,tipo,nome,razao_social,cpf,cnpj,telefone,observacoes,ativo")] Fornecedor fornecedor)
        {
            
            if (ModelState.IsValid)
            {

                _context.Add(fornecedor);
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
                return View(fornecedor);
            }
        }

        // GET: Fornecedor/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_fornecedor,tipo,nome,razao_social,cpf,cnpj,telefone,observacoes,ativo")] Fornecedor fornecedor)
        {
            if (id != fornecedor.id_fornecedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.id_fornecedor))
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
            return View(fornecedor);
        }

        // GET: Fornecedor/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.id_fornecedor == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // POST: Fornecedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.id_fornecedor == id);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterNomeFornecedor(string codigoFornecedor)
        {

            if (string.IsNullOrWhiteSpace(codigoFornecedor))
            {
                return Content("");
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(i => i.id_fornecedor.ToString() == codigoFornecedor);

            if (fornecedor == null)
            {
                return Content("");
            }

            var nomeFornecedor = string.IsNullOrEmpty(fornecedor.razao_social)
                ? $"{fornecedor.nome} | {fornecedor.cpf}"
                : $"{fornecedor.razao_social} | {fornecedor.cnpj}";

            return Content(nomeFornecedor);

        }

    }
}
