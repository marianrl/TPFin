using Microsoft.AspNetCore.Mvc;
using TPFin.Data;
using TPFin.Models;

namespace TPFin.Controllers
{
    public class LoginController : Controller
    {
        MyContext db = new MyContext();

        public const string SessionIdKey = "_id";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if(db.usuarios != null)
            {
                var user = db.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password);
                if (user != null)
                {
                    int[] userId = (from Usuario in db.usuarios
                              where Usuario.email == usuario.email && Usuario.password == usuario.password
                              select Usuario.id).ToArray();

                    if (userId != null && userId.ToString() != null)
                    {
                        HttpContext.Session.SetInt32(SessionIdKey, userId[0]);
                    }
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
