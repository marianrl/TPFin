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

            ViewData["Posts"] = postContext.ToList();
            _ = usuariosContext != null ?
                ViewData["Usuario"] = usuariosContext.ToList() :
                ViewData["Usuario"] = Enumerable.Empty<string>();
            return View();
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
