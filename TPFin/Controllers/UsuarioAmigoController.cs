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
    public class UsuarioAmigoController : Controller
    {
        private readonly MyContext _context;

        public UsuarioAmigoController(MyContext context)
        {
            _context = context;
        }

        // GET: UsuarioAmigo
        public async Task<IActionResult> Index()
        {
            var myContext = _context.UsuarioAmigo.Include(u => u.amigo).Include(u => u.user);
            return View(await myContext.ToListAsync());
        }

        // GET: UsuarioAmigo/Create
        public IActionResult Create()
        {
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id");
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id");
            return View();
        }

        // POST: UsuarioAmigo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioAmigo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idUser);
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idAmigo);
            return View(usuarioAmigo);
        }

        // GET: UsuarioAmigo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsuarioAmigo == null)
            {
                return NotFound();
            }

            var usuarioAmigo = await _context.UsuarioAmigo.FindAsync(id);
            if (usuarioAmigo == null)
            {
                return NotFound();
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idUser);
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idAmigo);
            return View(usuarioAmigo);
        }

        // POST: UsuarioAmigo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            
            if (_context.UsuarioAmigo == null)
            {
                return Problem("Entity set 'MyContext.UsuarioAmigo'  is null.");
            }
            UsuarioAmigo amigoUsuario = new UsuarioAmigo();
            amigoUsuario.idUser = usuarioAmigo.idAmigo;
            amigoUsuario.idAmigo = usuarioAmigo.idUser;

            _context.UsuarioAmigo.Add(usuarioAmigo);
            _context.UsuarioAmigo.Add(amigoUsuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioAmigoExists(usuarioAmigo.idAmigo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            TempData["Message"] = "Amigo agregado";
            return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioAmigo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsuarioAmigo == null)
            {
                return NotFound();
            }

            var usuarioAmigo = await _context.UsuarioAmigo
                .Include(u => u.amigo)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.idAmigo == id);
            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            return View(usuarioAmigo);
        }

        // POST: UsuarioAmigo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idAmigo)
        {

            if (_context.UsuarioAmigo == null)
            {
                return NotFound();
            }
            UsuarioAmigo usuarioAmigo = new UsuarioAmigo(id,idAmigo);
            UsuarioAmigo amigoUsuario = new UsuarioAmigo(idAmigo,id);           

            _context.UsuarioAmigo.Remove(usuarioAmigo);
            _context.UsuarioAmigo.Remove(amigoUsuario);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Amigo eliminado";
            return RedirectToAction("Index", "Home");
        }

        private bool UsuarioAmigoExists(int id)
        {
          return (_context.UsuarioAmigo?.Any(e => e.idAmigo == id)).GetValueOrDefault();
        }
    }
}
