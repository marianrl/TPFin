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
        public const string SessionEmailKey = "_email";
        public const string SessionPasswordKey = "_password";
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

                string[] userEmail = (from Usuario in db.usuarios
                                      where Usuario.email == usuario.email
                                      select Usuario.email).ToArray();

                if (user != null)
                {
                    IEnumerable<Usuario> userList = db.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password);

                    string email = userList.First().email;
                    string password = userList.First().password;
                    int idUser = userList.First().id;
                    string nombre = userList.First().nombre;
                    string apellido = userList.First().apellido;
                    bool isAdmin = userList.First().isAdm;
                    bool block = userList.First().bloqueado;
                    int intFallidos = userList.First().intentosFallidos;
                    

                    if (!isAdmin)
                    {
                        HttpContext.Session.SetString(SessionNyaKey, nombre + " " + apellido);
                        return RedirectToAction("IndexAdmin", "Home");
                    }
                    else
                    {
                        if (!block)
                        {
                            HttpContext.Session.SetInt32(SessionIdKey, idUser);
                            HttpContext.Session.SetString(SessionAdminKey, isAdmin.ToString());
                            HttpContext.Session.SetString(SessionNyaKey, nombre + " " + apellido);
                            HttpContext.Session.SetString(SessionBlockKey, block.ToString());
                            HttpContext.Session.SetInt32(SessionBlockKey, intFallidos);
                            HttpContext.Session.SetString(SessionPasswordKey, password);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["Message"] = "Usuario bloqueado, contacte al administrador";
                            return View();
                        }
                    }

                    
                }
                else
                {
                    if (userEmail[0] == usuario.email)
                    {
                        IEnumerable<Usuario> aux = db.usuarios.Where(x => x.email == usuario.email);
                        Usuario usuarioToUpdate = aux.First();
                        usuarioToUpdate.intentosFallidos++;
                        db.Update(usuarioToUpdate);
                        db.SaveChanges();

                        if (usuarioToUpdate.intentosFallidos >= 3)
                        {
                            usuarioToUpdate.bloqueado = true;
                            TempData["Message"] = "Usuario bloqueado, contacte al administrador";
                        }
                        else
                        {
                            TempData["Message"] = "Password mal ingresada, intente de nuevo";
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Usuario no encontrado, favor de registrarse";
                    }
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
