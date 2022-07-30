using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TPFin.Data;
using TPFin.Models;

namespace TPFin.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context; 
        
        public HomeController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var postContext = _context.post.Include(p => p.user);
            var usuariosContext = _context.usuarios;
            var userId = HttpContext.Session.GetInt32("_id");
            var amigosContext = _context.UsuarioAmigo;
            HttpContext.Session.GetString("_nombre");
            ViewData["Posts"] = postContext.ToList();
            _ = usuariosContext != null ?
                ViewData["Usuario"] = usuariosContext.ToList() :
                ViewData["Usuario"] = Enumerable.Empty<string>();
            ViewData["Amigos"] = amigosContext.ToList();
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("_id");
            HttpContext.Session.Remove("_nombre");
            HttpContext.Session.Remove("_admin");
            HttpContext.Session.Remove("_block");
            HttpContext.Session.Remove("_email");

            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
