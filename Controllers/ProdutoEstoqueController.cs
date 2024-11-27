using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForLifeWeb.Data;
using ForLifeWeb.Models;

namespace ForLifeWeb.Controllers
{
    public class ProdutoEstoqueController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoEstoqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProdutoEstoque
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.MovimentoProdutoEstoqueW.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.descrMovimento.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td>{item.idMovimentoProduto}</td>
                        <td>{item.nome}</td>
                        <td>{item.descrMovimento}</td>
                        <td>{item.quantidade_anterior}</td>
                        <td>{item.quantidadeMovimento}</td>
                        <td>{item.quantidade_atual}</td>
                        <td>{item.data_registro}</td>
                    </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        
        // GET: ProdutoEstoque/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produtoEstoque = await _context.ProdutoEstoque
                .FirstOrDefaultAsync(m => m.id_estoque == id);
            if (produtoEstoque == null)
            {
                return NotFound();
            }

            return View(produtoEstoque);
        }

        // POST: ProdutoEstoque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produtoEstoque = await _context.ProdutoEstoque.FindAsync(id);
            if (produtoEstoque != null)
            {
                _context.ProdutoEstoque.Remove(produtoEstoque);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoEstoqueExists(int id)
        {
            return _context.ProdutoEstoque.Any(e => e.id_estoque == id);
        }
    }
}
