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
    public class PlantioController : Controller
    {
        private readonly AppDbContext _context;

        public PlantioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Plantio
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var dados = _context.Plantio
                .Join(
                    _context.Produtos,
                    plantio => plantio.produto_id,
                    produto => produto.id_produto,
                    (plantio, produto) => new {Plantio = plantio, Produto = produto}
                )
                .Where(result => result.Plantio.data_baixa == null)
                .AsEnumerable()
                .Select(result => (result.Plantio, result.Produto))
                .ToList();


            return View(dados);
        }

        // GET: Plantio/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantio = await _context.Plantio
                .FirstOrDefaultAsync(m => m.id_plantio == id);
            if (plantio == null)
            {
                return NotFound();
            }

            return View(plantio);
        }

        // GET: Plantio/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plantio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_plantio,insumo_id,produto_id,quantidade_plantio,data_plantio,data_colheita,data_vencimento,data_registro,data_baixa")] Plantio plantio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantio);
        }

        // GET: Plantio/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantio = await _context.Plantio.FindAsync(id);
            if (plantio == null)
            {
                return NotFound();
            }
            return View(plantio);
        }

        // POST: Plantio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_plantio,insumo_id,produto_id,quantidade_plantio,data_plantio,data_colheita,data_vencimento,data_registro,data_baixa")] Plantio plantio)
        {
            if (id != plantio.id_plantio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantioExists(plantio.id_plantio))
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
            return View(plantio);
        }

        // GET: Plantio/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantio = await _context.Plantio
                .FirstOrDefaultAsync(m => m.id_plantio == id);
            if (plantio == null)
            {
                return NotFound();
            }

            return View(plantio);
        }

        // POST: Plantio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantio = await _context.Plantio.FindAsync(id);
            if (plantio != null)
            {
                _context.Plantio.Remove(plantio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantioExists(int id)
        {
            return _context.Plantio.Any(e => e.id_plantio == id);
        }
    }
}
