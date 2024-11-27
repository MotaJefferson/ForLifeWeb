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
            var query = _context.Insumos.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.descricao.Contains(searchTerm) ||
                    x.tipo.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td><input type='checkbox' class='select-cliente' value='{item.id_insumo}'></td>
                        <td>{item.id_insumo}</td>
                        <td>{item.nome}</td>
                        <td>{item.descricao}</td>
                        <td>{item.tipo}</td>
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
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Insert(int id, [Bind("insumo_id,fornecedor_id,quantidade")] InsumoCompra insumoCompra)
        {
            try
            {
                // Corrigindo a maneira de acessar os valores dos parâmetros no insumoEstoque
                Console.WriteLine("Insumo: " + insumoCompra.insumo_id);
                Console.WriteLine("Fornecedor: " + insumoCompra.fornecedor_id);
                Console.WriteLine("Quantidade: " + insumoCompra.quantidade);

                // Buscando o insumo no banco
                var insumo = await _context.Insumos.FirstOrDefaultAsync(i => i.id_insumo == insumoCompra.insumo_id);

                if (insumo == null)
                {
                    return NotFound("Insumo não encontrado.");
                }

                // Definindo as datas
                DateTime dataEntrada = DateTime.Now;
                DateTime dataVencimentoEstimado = dataEntrada.AddDays(insumo.periodo_vencimento ?? 0);

                char tipoMovimento = 'E';
                int? quantidadeEntrada = insumoCompra.quantidade;  

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

                // Executando o procedimento armazenado
                await _context.Database.ExecuteSqlRawAsync("EXEC AtualizaEstoqueInsumoMovimento @TipoMovimento, @Quantidade, @InsumoId, @FornecedorId, @DataEntrada, @DataVencimentoEstimado, @DataSaida", parametros);

                // Redirecionando para a ação "Index"
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Tratamento de exceção
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
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
