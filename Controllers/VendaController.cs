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
    public class VendaController : Controller
    {
        private readonly AppDbContext _context;

        public VendaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Venda
        [Authorize]
        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = from venda in _context.Vendas
                        join produto in _context.Produtos
                        on venda.produto_id equals produto.id_produto
                        join cliente in _context.Clientes
                        on venda.cliente_id equals cliente.id_cliente
                        select new
                        {
                            Venda = venda,
                            Produto = produto,
                            Cliente = cliente
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.Cliente.nome.Contains(searchTerm) ||
                    x.Venda.id_venda.ToString().Contains(searchTerm) ||
                    x.Produto.nome.Contains(searchTerm) ||
                    x.Venda.quantidade_venda.ToString().Contains(searchTerm));
            }

            var model = query
                .AsEnumerable()
                .Select(x => (Venda: x.Venda, Produto: x.Produto, Cliente: x.Cliente))
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                <tr class='venda-row'>
                    <td><input type='checkbox' class='select-venda' value='{item.Venda.id_venda}'></td>
                    <td>{item.Venda.id_venda}</td>
                    <td>{item.Cliente.nome}</td>
                    <td>{item.Produto.nome}</td>
                    <td>{item.Venda.quantidade_venda}</td>
                    <td>{item.Venda.preco_unitario.ToString("C2")}</td>
                    <td>{item.Venda.valor_venda.ToString("C2")}</td>
                    <td>{item.Venda.forma_pagamento}</td>
                    <td><span class='data-mask'>{item.Venda.data_venda:dd/MM/yyyy}</span></td>
                </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }


        // GET: Venda/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .FirstOrDefaultAsync(m => m.id_venda == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Venda/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_venda,produto_id,cliente_id,data_registro,quantidade_venda,data_venda,preco_unitario,valor_venda,forma_pagamento")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                venda.data_venda = DateTime.Now;
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venda);
        }

        // GET: Venda/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            Console.WriteLine(id);
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            return View(venda);
        }

        // POST: Venda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_venda,produto_id,cliente_id,numero_venda,data_registro,quantidade_venda,data_venda,preco_unitario,valor_venda")] Venda venda)
        {
            if (id != venda.id_venda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.id_venda))
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
            return View(venda);
        }

        // GET: Venda/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .FirstOrDefaultAsync(m => m.id_venda == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Venda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.id_venda == id);
        }
    }
}
