using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcTienda.Data;
using MvcTienda.Models;
using Microsoft.AspNetCore.Http;

namespace MvcTienda.Controllers
{
    public class ClientesController : Controller
    {
        private readonly MvcTiendaContexto _context;

        public ClientesController(MvcTiendaContexto context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var Cliente = await _context.Clientes.ToListAsync();
            var Pedido = await _context.Pedidos.ToListAsync();

            var clientes = from s in _context.Clientes
                            select s;
            int pageSize = 3;
            return View(await PaginatedList<Cliente>.CreateAsync(clientes.AsNoTracking(),
            pageNumber ?? 1, pageSize));

            //return View(Cliente);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["ListaPedidos"] = _context.Pedidos
                .Where(x => x.ClienteId == id)
                .Include(a => a.Estado)
                .OrderBy(c => c.Id).ToList();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Telefono,Direccion,Poblacion,CodigoPostal,Nif")] Cliente cliente)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Telefono,Direccion,Poblacion,CodigoPostal,Nif")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
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
                    if (!ClienteExists(cliente.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'MvcTiendaContexto.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return _context.Clientes.Any(e => e.Id == id);
        }
        // GET: MisAvisos
        public async Task<IActionResult> ListarPedidos()
        {
            // Se selecciona el empleado correspondiente al usuario actual
            var emailUsuario = User.Identity.Name;
            var cliente = await _context.Clientes.Where(e => e.Email == emailUsuario)
            .FirstOrDefaultAsync();
            if (cliente == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Se seleccionan los avisos del Empleado correspondiente al usuario actual
            var misPedidos = _context.Pedidos
            .Where(a => a.ClienteId == cliente.Id)
            .Include(a => a.Cliente);
            return View(await misPedidos.ToListAsync());
            // var mvcSoporteContexto = _context.Avisos.Include(a => a.Empleado)
            // .Include(a => a.Equipo).Include(a => a.TipoAveria);
            // return View(await mvcSoporteContexto.ToListAsync());
        }
    }
}
