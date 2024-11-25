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
    public class InsumoController : Controller
    {
        private readonly AppDbContext _context;

        public InsumoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Insumo
        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = from insumo in _context.Insumos
                        join estoque in _context.InsumoEstoque
                        on insumo.id_insumo equals estoque.insumo_id into estoqueGroup
                        from estoque in estoqueGroup.DefaultIfEmpty()
                        where insumo.ativo == true
                        select new
                        {
                            Insumo = insumo,
                            InsumoEstoque = estoque
                        };

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Insumo.nome.Contains(searchTerm));
            }

            var result = await query.ToListAsync();
            var model = result.Select(x => (Insumo: x.Insumo, InsumoEstoque: x.InsumoEstoque)).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Gerar o HTML diretamente no servidor e retornar
                var tableRows = model.Select(item => $@"
            <tr class='insumo-row'>
                <td><input type='checkbox' class='select-insumo' value='{item.Insumo.id_insumo}'></td>
                <td>{(item.InsumoEstoque != null ? item.InsumoEstoque.id_estoque.ToString() : "-")}</td>
                <td class='insumo-nome'>{item.Insumo.nome}</td>
                <td>{item.InsumoEstoque?.quantidade_atual ?? 0}</td>
            </tr>
            ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        
        }

        // GET: Insumo/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos
                .FirstOrDefaultAsync(m => m.id_insumo == id);
            if (insumo == null)
            {
                return NotFound();
            }

            return View(insumo);
        }

        // GET: Insumo/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insumo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_insumo,nome,descricao,tipo,ativo,periodo_vencimento")] Insumo insumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insumo);
        }

        // GET: Insumo/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo == null)
            {
                return NotFound();
            }
            return View(insumo);
        }

        // POST: Insumo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_insumo,nome,descricao,tipo,ativo,periodo_vencimento")] Insumo insumo)
        {
            if (id != insumo.id_insumo)
            {
                Console.WriteLine($"ID de URL: {id}, ID do Insumo: {insumo.id_insumo}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsumoExists(insumo.id_insumo))
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
            return View(insumo);
        }

        // GET: Insumo/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos
                .FirstOrDefaultAsync(m => m.id_insumo == id);
            if (insumo == null)
            {
                return NotFound();
            }

            return View(insumo);
        }

        // POST: Insumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo != null)
            {
                _context.Insumos.Remove(insumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsumoExists(int id)
        {
            return _context.Insumos.Any(e => e.id_insumo == id);
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
