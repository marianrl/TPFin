using Microsoft.AspNetCore.Mvc;
using TPFin.Data;
using TPFin.Models;

namespace TPFin.Controllers
{
    public class LoginController : Controller
    {
        MyContext db = new MyContext();

        public const string SessionIdKey = "_id";
        public const string SessionNyaKey = "_nombre";
        public const string SessionAdminKey = "_admin";
        public const string SessionBlockKey = "_block";
        public const string SessionIntFalKey = "_intfall";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if(db.usuarios != null)
            {
                var user = db.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password).SingleOrDefault();
                if (user != null)
                {
                    int[] userId = (from Usuario in db.usuarios
                              where Usuario.email == usuario.email && Usuario.password == usuario.password
                              select Usuario.id).ToArray();
                    string[] userName = (from Usuario in db.usuarios
                                        where Usuario.email == usuario.email && Usuario.password == usuario.password
                                        select Usuario.nombre).ToArray();
                    string[] userApe = (from Usuario in db.usuarios
                                        where Usuario.email == usuario.email && Usuario.password == usuario.password
                                        select Usuario.apellido).ToArray();
                    bool[] userAdmin = (from Usuario in db.usuarios
                                        where Usuario.email == usuario.email && Usuario.password == usuario.password
                                        select Usuario.isAdm).ToArray();
                    bool[] userBlock = (from Usuario in db.usuarios
                                        where Usuario.email == usuario.email && Usuario.password == usuario.password
                                        select Usuario.bloqueado).ToArray();
                    int[] userIntFal = (from Usuario in db.usuarios
                                        where Usuario.email == usuario.email && Usuario.password == usuario.password
                                        select Usuario.intentosFallidos).ToArray();

                    if (userId != null && userId.ToString() != null && userAdmin.ToString() != null)
                    {
                        HttpContext.Session.SetInt32(SessionIdKey, userId[0]);
                        HttpContext.Session.SetString(SessionAdminKey, userAdmin[0].ToString());
                        HttpContext.Session.SetString(SessionNyaKey, userName[0] + " " + userApe[0]);
                        HttpContext.Session.SetString(SessionBlockKey, userBlock[0].ToString());
                        HttpContext.Session.SetInt32(SessionBlockKey, userIntFal[0]);

                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Usuario no encontrado, favor de registrarse";
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
