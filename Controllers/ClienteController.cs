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
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.Clientes.AsQueryable();

            // Se houver um termo de pesquisa, aplica o filtro
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.nome.Contains(searchTerm) ||
                    x.cpf.Contains(searchTerm) ||
                    x.telefone.Contains(searchTerm));
            }

            var model = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var tableRows = model.Select(item => $@"
                    <tr class='produto-row'>
                        <td><input type='checkbox' class='select-cliente' value='{item.id_cliente}'></td>
                        <td>{item.id_cliente}</td>
                        <td>{item.nome}</td>
                        <td>{item.cpf}</td>
                        <td>{item.telefone}</td>
                        <td>{item.observacoes}</td>
                    </tr>
                ");

                return Content(string.Join("", tableRows), "text/html");
            }

            return View(model);
        }

        [Authorize]
        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id_cliente,nome,telefone,cpf,observacoes,ativo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id_cliente,nome,telefone,cpf,observacoes,ativo")] Cliente cliente)
        {

            if (id != cliente.id_cliente)
            {
                Console.WriteLine($"ID de URL: {id}, ID do Produto: {cliente.id_cliente}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.id_cliente))
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

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterNomeCliente(string codigoCliente)
        {

            if (string.IsNullOrWhiteSpace(codigoCliente))
            {
                return Content("");
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(i => i.id_cliente.ToString() == codigoCliente);

            if (cliente == null)
            {
                return Content("");
            }

            var nomeDescricao = string.IsNullOrWhiteSpace(cliente.cpf)
                ? cliente.nome
                : $"{cliente.nome} - {cliente.cpf}";

            return Content(nomeDescricao);

        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.id_cliente == id);
        }
    }
}
