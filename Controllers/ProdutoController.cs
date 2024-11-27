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
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Produto
        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.Produtos.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.descricao.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td><input type='checkbox' class='select-cliente' value='{item.id_produto}'></td>
                        <td>{item.id_produto}</td>
                        <td>{item.nome}</td>
                        <td>{item.descricao}</td>
                    </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        // GET: Produto/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.id_produto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_produto,insumo_id,nome,descricao,periodo_vencimento,periodo_colheita,periodo_limite_colheita,ativo")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_produto,insumo_id,nome,descricao,periodo_vencimento,periodo_colheita,periodo_limite_colheita,ativo")] Produto produto)
        {

            if (id != produto.id_produto)
            {
                Console.WriteLine($"ID de URL: {id}, ID do Produto: {produto.id_produto}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.id_produto))
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

            return View(produto);
        }

        // GET: Produto/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.id_produto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.id_produto == id);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterNomeInsumo(string codigoInsumo)
        {

            if (string.IsNullOrWhiteSpace(codigoInsumo))
            {
                return Content("");
            }

            var insumo = await _context.Insumos
                .FirstOrDefaultAsync(i => i.id_insumo.ToString() == codigoInsumo);

            if (insumo == null)
            {
                return Content("");
            }

            var nomeDescricao = string.IsNullOrWhiteSpace(insumo.descricao)
                ? insumo.nome
                : $"{insumo.nome} - {insumo.descricao}";

            return Content(nomeDescricao);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterNomeProduto(string codigoProduto)
        {

            if (string.IsNullOrWhiteSpace(codigoProduto))
            {
                return Content("");
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(i => i.id_produto.ToString() == codigoProduto);

            if (produto == null)
            {
                return Content("");
            }

            var nomeDescricao = string.IsNullOrWhiteSpace(produto.descricao)
                ? produto.nome
                : $"{produto.nome} - {produto.descricao}";

            return Content(nomeDescricao);

        }

        [HttpPost]
        [Authorize]
        public IActionResult RedirectBasedOnSelection(List<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
            {
                return RedirectToAction("Create");
            }
            else if (selectedIds.Count == 1)
            {
                return RedirectToAction("Edit", new { id = selectedIds[0] });
            }
            else
            {
                TempData["ErrorMessage"] = "Você não pode selecionar mais de um produto para editar.";
                return RedirectToAction("Index");
            }
        }

    }
}
