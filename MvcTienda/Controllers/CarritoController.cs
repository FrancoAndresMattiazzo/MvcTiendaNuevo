using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTienda.Data;
using MvcTienda.Models;
using System.Data;

namespace MvcTienda.Controllers
{
    public class CarritoController : Controller
    {
        private readonly MvcTiendaContexto _context;

        public CarritoController(MvcTiendaContexto context)
        {
            _context = context;
        }
        /*
        public async Task<IActionResult> Index()
        {
             int intNumeroPedido = 0;
             string numeroPedido = HttpContext.Session.GetString("NumPedido");
             if (numeroPedido == null)
             {

                 return View("CarritoVacio");

             }

             intNumeroPedido = Convert.ToInt32(numeroPedido);
             var pedido = await _context.Pedidos
                                 .Include(x => x.Cliente)
                                 .Include(x => x.Estado)
                                 .Include(x => x.Detalles)
                                 .ThenInclude(x => x.Producto)
                                 .FirstOrDefaultAsync(e => e.Id == intNumeroPedido);

             if (pedido == null)
             {
                 return NotFound();
             }
             return View(pedido);
            string emailUsuario = User.Identity.Name;
            int ClienteId;
            Pedido pedidoActual = null;
            var pendiente = await _context.Estados
                .Where(e => e.Descripcion == "Pendiente")
                .FirstOrDefaultAsync();
            var cliente = await _context.Clientes
                .Where(e => e.Email== emailUsuario)
                .FirstOrDefaultAsync();
            if (cliente != null)
            {
                ClienteId = cliente.Id;

                pedidoActual = await _context.Pedidos
                    .Include(p => p.Detalles)
                    .ThenInclude(x => x.Producto)
                    .Include(p => p.Cliente)
                    .Where(e => e.ClienteId == ClienteId && e.EstadoId == pendiente.Id)
                    .FirstOrDefaultAsync();
            }
            return View(pedidoActual);
        }

        //GET Carrito Vacio

        public ActionResult CarritoVacio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EliminarLinea(int id)
        {
            var detalle = await _context.Detalles.FindAsync(id);
            _context.Detalles.Remove(detalle);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Carrito/MasCantidad

        private bool DetalleExists(int id)
        {
            return _context.Detalles.Any(p => p.Id == id);
        }

        public async Task<IActionResult> MasCantidad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.Detalles.FindAsync(id);
            detalle.Cantidad = detalle.Cantidad + 1;

            var producto = await _context.Productos.FindAsync(detalle.ProductoId);
            producto.Stock = producto.Stock - 1;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleExists(detalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET Carrito/menos cantidad

        public async Task<IActionResult> MenosCantidad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.Detalles.FindAsync(id);
            detalle.Cantidad = detalle.Cantidad - 1;

            var producto = await _context.Productos.FindAsync(detalle.ProductoId);
            producto.Stock = producto.Stock + 1;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleExists(detalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //POST Carrito/ConfirmarPedido
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ConfirmarPedido(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);

            // Se cambia el estado del pedido a confirmado

            pedido.EstadoId = 2;
            pedido.Confirmado = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                    // Al confirmar el pedido se pone la variable de sesion del pedido actual a null
                    HttpContext.Session.Remove("NumPedido");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id)) { return NotFound(); }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index), "Escaparate");
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(p => p.Id == id);
        }*/
        public async Task<IActionResult> Index()
        {
            int intNumeroPedido = 0;
            string numeroPedido = HttpContext.Session.GetString("NumPedido");
            if (numeroPedido == null)
            {

                return View("CarritoVacio");

            }

            intNumeroPedido = Convert.ToInt32(numeroPedido);
            var pedido = await _context.Pedidos
                                .Include(x => x.Cliente)
                                .Include(x => x.Estado)
                                .Include(x => x.Detalles)
                                .ThenInclude(x => x.Producto)
                                .FirstOrDefaultAsync(e => e.Id == intNumeroPedido);

            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // GET Carrito Vacio

        public ActionResult CarritoVacio()
        {
            return View();
        }

        // POST Detalles/EliminarLinea

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarLinea(int id)
        {
            var detalle = await _context.Detalles.FindAsync(id);
            _context.Detalles.Remove(detalle);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        // GET Carrito/MasCantidad

        private bool DetalleExists(int id)
        {
            return _context.Detalles.Any(p => p.Id == id);
        }

        public async Task<IActionResult> MasCantidad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.Detalles.FindAsync(id);
            detalle.Cantidad = detalle.Cantidad + 1;

            var producto = await _context.Productos.FindAsync(detalle.ProductoId);
            producto.Stock = producto.Stock - 1;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleExists(detalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET Carrito/MenosCantidad

        public async Task<IActionResult> MenosCantidad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _context.Detalles.FindAsync(id);
            detalle.Cantidad = detalle.Cantidad - 1;

            var producto = await _context.Productos.FindAsync(detalle.ProductoId);
            producto.Stock = producto.Stock + 1;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleExists(detalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            return RedirectToAction(nameof(Index));
        }

        //POST Carrito/ConfirmarPedido

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarPedido(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);


            // Se cambia el estado del pedido a confirmado

            pedido.EstadoId = 2;
            pedido.Confirmado = DateTime.Now;

            return RedirectToAction(nameof(Pagar));
            // return View("Pagar");
        }

        public async Task<IActionResult> Pagar()
        {
            int intNumeroPedido = 0;
            string numeroPedido = HttpContext.Session.GetString("NumPedido");
            if (numeroPedido == null)
            {

                return View("CarritoVacio");

            }

            intNumeroPedido = Convert.ToInt32(numeroPedido);
            var pedido = await _context.Pedidos
                                .Include(x => x.Cliente)
                                .Include(x => x.Estado)
                                .Include(x => x.Detalles)
                                .ThenInclude(x => x.Producto)
                                .FirstOrDefaultAsync(e => e.Id == intNumeroPedido);

            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);

            // Se cambia el estado del pedido a confirmado

            pedido.EstadoId = 3;
            pedido.Confirmado = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                    // Al confirmar el pedido se pone la variable de sesion del pedido actual a null
                    HttpContext.Session.Remove("NumPedido");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index), "Escaparate");
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(p => p.Id == id);
        }

    }
}
