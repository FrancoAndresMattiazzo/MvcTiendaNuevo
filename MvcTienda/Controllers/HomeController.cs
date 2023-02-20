using Microsoft.AspNetCore.Mvc;
using MvcTienda.Data;
using MvcTienda.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace MvcTienda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MvcTiendaContexto _context;

        public HomeController(ILogger<HomeController> logger, MvcTiendaContexto context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string? emailUsuario = User.Identity.Name;
            Cliente? cliente = _context.Clientes.Where(e => e.Email == emailUsuario)
            .FirstOrDefault();
            if (User.Identity.IsAuthenticated &&
            User.IsInRole("Usuario") &&
            cliente == null)
            {
                return RedirectToAction("Create", "MisDatos");
            }
            return RedirectToAction("Index", "Escaparate");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}