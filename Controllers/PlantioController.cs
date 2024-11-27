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
using Microsoft.Data.SqlClient;

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
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = from plantio in _context.Plantio
                        join produto in _context.Produtos
                        on plantio.produto_id equals produto.id_produto
                        select new
                        {
                            Plantio = plantio,
                            Produto = produto
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.Produto.nome.Contains(searchTerm) ||
                    x.Plantio.id_plantio.ToString().Contains(searchTerm) ||
                    x.Plantio.quantidade_plantio.ToString().Contains(searchTerm));
            }

            // Corrigido: Seleção com projeção correta
            var model = query
                .AsEnumerable()
                .Select(x => (Plantio: x.Plantio, Produto: x.Produto))
                .ToList();


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                <tr class='produto-row'>
                    <td><input type='checkbox' class='select-plantacao' value='{item.Plantio.id_plantio}'></td>
                    <td>{item.Plantio.id_plantio}</td>
                    <td>{item.Produto.nome}</td>
                    <td>{item.Plantio.quantidade_plantio}</td>
                    <td><span class='data-mask'>{item.Plantio.data_plantio:dd/MM/yyyy}</span></td>
                    <td><span class='data-mask'>{item.Plantio.data_vencimento:dd/MM/yyyy}</span></td>
                </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        // POST: InsumoCompra/Plantar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Plantar([Bind("insumo_id,produto_id,quantidade_plantio,data_plantio,data_vencimento,data_registro")] Plantio plantio)
        {
            Console.WriteLine($"Insumo ID: {plantio.insumo_id}");
            Console.WriteLine($"Produto ID: {plantio.produto_id}");
            Console.WriteLine($"Quantidade Plantio: {plantio.quantidade_plantio}");

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"Produto ID: {plantio.produto_id}");
                    Console.WriteLine($"Quantidade Plantio: {plantio.quantidade_plantio}");

                    Console.WriteLine($"{entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                
                var insumoEstoque = await _context.InsumoEstoque.FirstOrDefaultAsync(i => i.insumo_id == plantio.insumo_id);
                var produto = await _context.Produtos.FirstOrDefaultAsync(i => i.id_produto == plantio.produto_id);

                
                DateTime dataSaida = DateTime.Now;
                DateTime dataVencimentoEstimado = dataSaida.AddDays(produto.periodo_vencimento);

                //DADOS DO PLANTIO
                plantio.data_plantio = DateTime.Now;
                plantio.data_vencimento = dataVencimentoEstimado;

                char tipoMovimento = 'S';
                int quantidadeSaida = plantio.quantidade_plantio;

                var parametros = new[]
                {
                    new SqlParameter("@TipoMovimento", tipoMovimento),
                    new SqlParameter("@Quantidade", quantidadeSaida),
                    new SqlParameter("@InsumoId", insumoEstoque.insumo_id),
                    new SqlParameter("@FornecedorId", insumoEstoque.fornecedor_id),
                    new SqlParameter("@DataEntrada", DBNull.Value),
                    new SqlParameter("@DataVencimentoEstimado", dataVencimentoEstimado),
                    new SqlParameter("@DataSaida", dataSaida)
                };

                try
                {
                    int resultado = await _context.Database.ExecuteSqlRawAsync(
                        "EXEC AtualizaEstoqueInsumoMovimento @TipoMovimento, @Quantidade, @InsumoId, @FornecedorId, @DataEntrada, @DataVencimentoEstimado, @DataSaida",
                        parametros
                    );

                    if (resultado == 0)
                    {
                        return Json(new { success = false, message = "Falha ao atualizar o estoque no banco de dados." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Erro ao realizar operação: {ex.Message}" });
                }

                _context.Add(plantio);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Plantio");
            }

            return View(plantio);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Colher([FromBody] ColherRequest request)
        {
            if (request.PlantiosIds == null || !request.PlantiosIds.Any())
            {
                return Json(new { success = false, message = "Nenhum item selecionado." });
            }

            try
            {
                foreach (var plantioId in request.PlantiosIds)
                {
                    var plantio = await _context.Plantio.FindAsync(plantioId);
                    if (plantio == null)
                    {
                        return Json(new { success = false, message = $"Plantio com ID {plantioId} não encontrado." });
                    }

                    var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.id_produto == plantio.produto_id);

                    if (produto == null)
                    {
                        return Json(new { success = false, message = $"Produto para o plantio ID {plantioId} não encontrado." });
                    }

                    // Lógica para colher e estocar
                    DateTime dataEntrada = DateTime.Now;
                    DateTime dataVencimentoEstimado = dataEntrada.AddDays(produto.periodo_vencimento);

                    plantio.data_colheita = DateTime.Now;

                    char tipoMovimento = 'E'; // Movimentação de entrada no estoque
                    int quantidade = plantio.quantidade_plantio;

                    var parametros = new[]
                    {
                    new SqlParameter("@TipoMovimento", tipoMovimento),
                    new SqlParameter("@Quantidade", quantidade),
                    new SqlParameter("@ProdutoId", plantio.produto_id),
                    new SqlParameter("@DataColheita", plantio.data_colheita),
                    new SqlParameter("@DataVencimentoEstimado", dataVencimentoEstimado),
                    new SqlParameter("@DataSaida", DBNull.Value)
                };

                    int resultado = await _context.Database.ExecuteSqlRawAsync(
                        "EXEC AtualizaEstoqueProdutoMovimento @TipoMovimento, @Quantidade, @ProdutoId, @DataColheita, @DataVencimentoEstimado, @DataSaida",
                        parametros
                    );

                    if (resultado == 0)
                    {
                        return Json(new { success = false, message = "Falha ao atualizar o estoque no banco de dados." });
                    }

                    _context.Add(plantio);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Produtos colhidos e estocados com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao processar a colheita: {ex.Message}" });
            }
        }

        public class ColherRequest
        {
            public List<int> PlantiosIds { get; set; }
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
