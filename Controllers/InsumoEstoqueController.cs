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
    public class InsumoEstoqueController : Controller
    {
        private readonly AppDbContext _context;

        public InsumoEstoqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: InsumoEstoque
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.MovimentoInsumoEstoqueW.AsQueryable();

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
                    <tr class='insumo-row'>
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

        // GET: InsumoEstoque/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumoEstoque = await _context.InsumoEstoque
                .FirstOrDefaultAsync(m => m.id_estoque == id);
            if (insumoEstoque == null)
            {
                return NotFound();
            }

            return View(insumoEstoque);
        }

        // POST: InsumoEstoque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insumoEstoque = await _context.InsumoEstoque.FindAsync(id);
            if (insumoEstoque != null)
            {
                _context.InsumoEstoque.Remove(insumoEstoque);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsumoEstoqueExists(int id)
        {
            return _context.InsumoEstoque.Any(e => e.id_estoque == id);
        }
    }
}
