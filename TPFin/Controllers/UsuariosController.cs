using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFin.Data;

namespace TPFin.Models
{
    public class UsuariosController : Controller
    {
        private readonly MyContext _context;

        public UsuariosController(MyContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
              return _context.usuarios != null ? 
                          View(await _context.usuarios.ToListAsync()) :
                          Problem("Entity set 'MyContext.usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,dni,nombre,apellido,email,password,intentosFallidos,bloqueado,isAdm")] Usuario usuario)
        {
            int mailExiste = _context.usuarios.Where(x => x.email == usuario.email).Count();
            if (mailExiste > 0)
            {
                TempData["MessagemailExiste"] = "Mail ya registrado, intente con otro";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["MessageLoger"] = "Usuario registrado";
                return RedirectToAction(nameof(Index), "Login");
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios.FindAsync(id);
            usuario.password = "";

            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,dni,nombre,apellido,email,password,intentosFallidos,bloqueado,isAdm")] Usuario usuario, string current, string newP)
        {
            if (id != usuario.id)
            {
                return NotFound();
            }

            try
            {
                var admin = HttpContext.Session.GetString("_admin");
                var intFal = HttpContext.Session.GetInt32("_intfall");
                var block = HttpContext.Session.GetString("_block");

                _ = admin == "true" ? usuario.isAdm = true : usuario.isAdm = false;
                _ = intFal == 0 ? usuario.intentosFallidos = 0 : usuario.intentosFallidos = 1;
                _ = block == "true" ? usuario.bloqueado = true : usuario.bloqueado = false;

                if(HttpContext.Session.GetString("_password") == current)
                {
                    if(newP == usuario.password)
                    {
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Usuario modificado correctamente";
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        TempData["Message"] = "La nueva password no coincide";
                        return RedirectToAction(nameof(Edit));
                    }
                }
                else
                {
                    TempData["Message"] = "Password actual incorrecta";
                    return RedirectToAction(nameof(Index), "Home");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.usuarios == null)
            {
                return Problem("Entity set 'MyContext.usuarios'  is null.");
            }

            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario != null)
            {
                foreach (Post a in _context.post)
                {
                    if (a.idUser == usuario.id)
                    {
                        _context.post.Remove(a);
                    }
                }
                _context.usuarios.Remove(usuario);
            }

            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.usuarios?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
