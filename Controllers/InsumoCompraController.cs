using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForLifeWeb.Data;
using ForLifeWeb.Models;
using Microsoft.Data.SqlClient;


namespace ForLifeWeb.Controllers
{
    public class InsumoCompraController : Controller
    {
        private readonly AppDbContext _context;

        public InsumoCompraController(AppDbContext context)
        {
            _context = context;
        }

        // GET: InsumoCompra/Create
        public IActionResult Index()
        {
            return View();
        }

        // POST: InsumoCompra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("fornecedor_id,insumo_id,quantidade")] InsumoCompra insumoCompra)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"{entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }


            if (ModelState.IsValid)
            {
                
                var insumo = await _context.Insumos.FirstOrDefaultAsync(i => i.id_insumo == insumoCompra.insumo_id);

                DateTime dataEntrada = DateTime.Now;
                DateTime dataVencimentoEstimado = dataEntrada.AddDays(insumo.periodo_vencimento ?? 0);

                char tipoMovimento = 'E';
                int quantidadeEntrada = insumoCompra.quantidade;

                var parametros = new
                {
                    TipoMovimento = tipoMovimento,
                    Quantidade = quantidadeEntrada,
                    InsumoId = insumoCompra.insumo_id,
                    FornecedorId = insumoCompra.fornecedor_id,
                    DataEntrada = dataEntrada,
                    DataVencimentoEstimado = dataVencimentoEstimado,
                    DataSaida = (DateTime?)null
                };

                await _context.Database.ExecuteSqlRawAsync(
                "EXEC AtualizaEstoqueInsumoMovimento @TipoMovimento, @Quantidade, @InsumoId, @FornecedorId, @DataEntrada, @DataVencimentoEstimado, @DataSaida",
                new[]
                {
                    new SqlParameter("@TipoMovimento", parametros.TipoMovimento),
                    new SqlParameter("@Quantidade", parametros.Quantidade),
                    new SqlParameter("@InsumoId", parametros.InsumoId),
                    new SqlParameter("@FornecedorId", parametros.FornecedorId),
                    new SqlParameter("@DataEntrada", parametros.DataEntrada),
                    new SqlParameter("@DataVencimentoEstimado", parametros.DataVencimentoEstimado == null),
                    new SqlParameter("@DataSaida",
                        parametros.DataSaida ?? (object)DBNull.Value) // Aceita nulo
                    }
                );

                _context.Add(insumoCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "InsumoEstoque");
            }

            return View(insumoCompra);
        }

        // GET: InsumoCompra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumoCompra = await _context.InsumoCompra
                .FirstOrDefaultAsync(m => m.id_compra == id);
            if (insumoCompra == null)
            {
                return NotFound();
            }



            return View(insumoCompra);
        }

        // POST: InsumoCompra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insumoCompra = await _context.InsumoCompra.FindAsync(id);
            if (insumoCompra != null)
            {
                _context.InsumoCompra.Remove(insumoCompra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsumoCompraExists(int id)
        {
            return _context.InsumoCompra.Any(e => e.id_compra == id);
        }
    }
}
