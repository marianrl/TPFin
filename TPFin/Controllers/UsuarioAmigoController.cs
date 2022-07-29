using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPFin.Data;
using TPFin.Models;

namespace TPFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAmigoController : Controller
    {
        private readonly MyContext _context;

        public UsuarioAmigoController(MyContext context)
        {
            _context = context;
        }

        // GET: api/UsuarioAmigo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioAmigo>>> GetUsuarioAmigo()
        {
            if (_context.UsuarioAmigo == null)
            {
                return NotFound();
            }
            return await _context.UsuarioAmigo.ToListAsync();
        }

        // GET: api/UsuarioAmigo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioAmigo>> GetUsuarioAmigo(int id)
        {
            if (_context.UsuarioAmigo == null)
            {
                return NotFound();
            }
            var usuarioAmigo = await _context.UsuarioAmigo.FindAsync(id);

            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            return usuarioAmigo;
        }


        // POST: api/UsuarioAmigo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioAmigo>> PostUsuarioAmigo(int id, int idAmigo)
        {
            UsuarioAmigo amigo = new UsuarioAmigo(id,idAmigo);
            UsuarioAmigo amegoRell = new UsuarioAmigo(idAmigo, id);
            if (_context.UsuarioAmigo == null)
            {
                return Problem("Entity set 'MyContext.UsuarioAmigo'  is null.");
            }
            _context.UsuarioAmigo.Add(amigo);
            _context.UsuarioAmigo.Add(amegoRell);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioAmigoExists(amigo.idAmigo))
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

        // DELETE: api/UsuarioAmigo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioAmigo(int id, int idAmigo)
        {
            if (_context.UsuarioAmigo == null)
            {
                return NotFound();
            }
            var amigo = await _context.UsuarioAmigo.FindAsync(id);
            var amegoRell = await _context.UsuarioAmigo.FindAsync(idAmigo);

            if (amigo == null && amegoRell == null)
            {
                return NotFound();
            }

            _context.UsuarioAmigo.Remove(amigo);
            _context.UsuarioAmigo.Remove(amegoRell);
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
