﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcTienda.Data;
using MvcTienda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MvcTienda.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductoesController : Controller
    {
        private readonly MvcTiendaContexto _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoesController(MvcTiendaContexto context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            var mvcTiendaContexto = _context.Productos.Include(p => p.Categoria);
            return View(await mvcTiendaContexto.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Texto,Precio,PrecioCadena,Stock,Escaparate,Imagen,CategoriaId")] Producto producto, IFormFile imagen)
        {
            int UltimoProducto = (from prod in _context.Productos select prod.Id).Max();
            UltimoProducto = UltimoProducto + 1;

            string strRutaImagenes = Path.Combine(_webHostEnvironment.WebRootPath, "./imagenes");
            string strExtension = Path.GetExtension(imagen.FileName);
            string strNombreFichero = UltimoProducto.ToString() + strExtension;
            string strRutaFichero = Path.Combine(strRutaImagenes, strNombreFichero);
            using (var fileStream = new FileStream(strRutaFichero, FileMode.Create))
            {
                imagen.CopyTo(fileStream);
            }
            // Actualizar producto con nueva imagen
            producto.Imagen = strNombreFichero;
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }

        /*if (id == null)
            {
                return NotFound();
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            if (imagen == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Copiar archivo de imagen
                string strRutaImagenes = Path.Combine(_webHostEnvironment.WebRootPath, "/imagenes");
                string strExtension = Path.GetExtension(imagen.FileName);
                string strNombreFichero = producto.Id.ToString() + strExtension;
                string strRutaFichero = Path.Combine(strRutaImagenes, strNombreFichero);
                using (var fileStream = new FileStream(strRutaFichero, FileMode.Create))
                {
                    imagen.CopyTo(fileStream);
                }
                // Actualizar producto con nueva imagen
                producto.Imagen = strNombreFichero;
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(producto);*/

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Texto,Precio,PrecioCadena,Stock,Escaparate,Imagen,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }
        // GET: Productos/CambiarImagen/5
        public async Task<IActionResult> CambiarImagen(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }
            var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
        // POST: Productos/CambiarImagen/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarImagen(int? id, IFormFile imagen)
        {
            if (id == null)
            {
                return NotFound();
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            if (imagen == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Copiar archivo de imagen
                string strRutaImagenes = Path.Combine(_webHostEnvironment.WebRootPath, "./imagenes");
                string strExtension = Path.GetExtension(imagen.FileName);
                string strNombreFichero = producto.Id.ToString() + strExtension;
                string strRutaFichero = Path.Combine(strRutaImagenes, strNombreFichero);
                using (var fileStream = new FileStream(strRutaFichero, FileMode.Create))
                {
                    imagen.CopyTo(fileStream);
                }
                // Actualizar producto con nueva imagen
                producto.Imagen = strNombreFichero;
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(producto);
        }
                // GET: Productoes/Delete/5
                public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'MvcTiendaContexto.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return _context.Productos.Any(e => e.Id == id);
        }
    }
}
