using Microsoft.AspNetCore.Mvc;
using TPFin.Data;
using TPFin.Models;

namespace TPFin.Controllers
{
    public class LoginController : Controller
    {
        MyContext db = new MyContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if(db.usuarios != null)
            {
                var user = db.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password).Count();
                if (user > 0)
                {
                    //ISession
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}
