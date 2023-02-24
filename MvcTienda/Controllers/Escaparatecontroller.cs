using Microsoft.AspNetCore.Mvc;
using MvcTienda.Data;
using MvcTienda;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MvcTienda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MvcTienda.Controllers
{
    public class Escaparatecontroller : Controller
    {
        private readonly MvcTiendaContexto _context;

        public Escaparatecontroller(MvcTiendaContexto context)
        {
            _context = context;
        }

        //GET: Escaparate

        public async Task<ActionResult> Index(int? id)
        {
            var productos = _context.Productos.AsQueryable();

            if(id == null)
            {
                //selecciona productos del escaparate
                productos = productos.Where(x => x.Escaparate == true);
            }
            else
            {
                //selecciona productos de la categoria id
                productos = productos.Where(x => x.CategoriaId == id);

                //Obtiene el nombre de la categoria seleccionada
                ViewBag.DescripocionCategoria = _context.Categorias.Find(id).Descripcion.ToString();
            }

            ViewData["ListaCategorias"] = _context.Categorias.OrderBy(c => c.Descripcion).ToList();

            productos = productos.Include(x => x.Categoria)
                .Where(x => x.Escaparate == true)
                .Where(x => x.Stock > 0);
         
 

            return View(await productos.ToListAsync());
        }
        /*
        //GET AñadirCarrito

        public async Task<IActionResult> AñadirCarrito(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Escaparate/AgregarCarrito/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AñadirCarrito(int id)
        {
            // Cargar datos de producto a añadir al carrito
            var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            // Crear objetos pedido y detalle a agregar
            Pedido pedido = new Pedido();
            Detalle detalle = new Detalle();

            Cliente usuario = await _context.Clientes.Where(p => p.Email == User.Identity.Name).FirstOrDefaultAsync();

            if (HttpContext.Session.GetString("NumPedido") == null)
            {
                pedido.Fecha = DateTime.Now;
                pedido.Confirmado = null;
                pedido.Preparado = null;
                pedido.Enviado = null;
                pedido.Cobrado = null;
                pedido.Devuelto = null;
                pedido.Anulado = null;
                pedido.ClienteId = usuario.Id;
                pedido.EstadoId = 1;
                if (ModelState.IsValid)
                {
                    _context.Add(pedido);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.SetString("NumPedido", pedido.Id.ToString());
            }
            // Agregar producto al detalle de un pedido existente
            string strNumeroPedido = HttpContext.Session.GetString("NumPedido");
            detalle.PedidoId = Convert.ToInt32(strNumeroPedido);
            // El valor de id tiene el Id del producto a agregar
            detalle.ProductoId = id;
            detalle.Cantidad = 1;
            detalle.Precio = producto.Precio;
            detalle.Descuento = 0;
            if (ModelState.IsValid)
            {
                _context.Add(detalle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }*/
        //GET AñadirCarrito
        public async Task<IActionResult> AñadirCarrito(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                            .Where(x => x.Escaparate == true)
                            .Include(p => p.Categoria)
                            .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            ViewData["ListaProductos"] = _context.Productos
                .Where( x => x.Escaparate == true)
                .Where( x => x.Categoria == producto.Categoria)
                .Where( x => x.Id != producto.Id)
                .Where(x => x.Stock > 0)
                .OrderBy(c => c.Descripcion).ToList();

            return View(producto);
        }




        // POST: Escaparate/AgregarCarrito/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AñadirCarrito(int id, string? strCupon)
        {
            // Cargar datos de producto a añadir al carrito
            var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            //Aplicar descuento
            var Cupon = strCupon;
            decimal descuentoaplicar = 0;
            if (Cupon == "" || Cupon == null)
            {
                descuentoaplicar = 0;

            }
            if (Cupon == "Aplicar50")
            {
                descuentoaplicar = 50;
            }

            //Al añadir un producto, deberemos quitarle 1 unidad de stock que quedará reservada para el
            //cliente hasta que realice otra operación
            producto.Stock--;

            if (producto.Stock <= 0)
            {
                producto.Escaparate = false;
            }

            // Crear objetos pedido y detalle a agregar
            Pedido pedido = new Pedido();
            Detalle detalle = new Detalle();

            var email = User.Identity.Name;

            var usuario = await _context.Clientes.FirstOrDefaultAsync(p => p.Email == User.Identity.Name);

            if (usuario == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            if (HttpContext.Session.GetString("NumPedido") == null)
            {
                pedido.Fecha = DateTime.Now;
                pedido.Confirmado = null;
                pedido.Preparado = null;
                pedido.Enviado = null;
                pedido.Cobrado = null;
                pedido.Devuelto = null;
                pedido.Anulado = null;
                pedido.ClienteId = usuario.Id;
                pedido.EstadoId = 1;
                if (ModelState.IsValid)
                {
                    _context.Add(pedido);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.SetString("NumPedido", pedido.Id.ToString());
            }
            // Agregar producto al detalle de un pedido existente
            string strNumeroPedido = HttpContext.Session.GetString("NumPedido");
            detalle.PedidoId = Convert.ToInt32(strNumeroPedido);
            // El valor de id tiene el Id del producto a agregar
            detalle.ProductoId = id;
            detalle.Cantidad = 1;
            detalle.Precio = producto.Precio;
            detalle.Descuento = descuentoaplicar;
            if (ModelState.IsValid)
            {
                _context.Add(detalle);
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<IActionResult> Descuento(string strCupon, int id)
        {
            var Cupon = strCupon.Trim();
            decimal descuentoaplicar = 0;
            var producto = await _context.Productos
                            .Where(x => x.Escaparate == true)
                            .Include(p => p.Categoria)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (Cupon == "" || Cupon == null)
            {              
                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }
            if(Cupon == "Aplicar50")
            {
                descuentoaplicar = 50;
            }
            var detalle = await _context.Detalles.FindAsync(id);
            detalle.Descuento = descuentoaplicar;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
            }

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }*/

    }
}
