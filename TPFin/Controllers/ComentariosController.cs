﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFin.Data;

namespace TPFin.Models
{
    public class ComentariosController : Controller
    {
        private readonly MyContext _context;

        public ComentariosController(MyContext context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index()
        {
            var myContext = _context.comentarios.Include(c => c.post).Include(c => c.usuario);
            return View(await myContext.ToListAsync());
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.comentarios
                .Include(c => c.post)
                .Include(c => c.usuario)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // GET: Comentarios/Create
        public IActionResult Create(int id)
        {

            ViewData["idPost"] = id;
            ViewData["idUser"] = "";
            return View();
        }

        // POST: Comentarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idPost,idUser,contenido,fecha")] Comentario comentario, int idp, int idu)
        {
            DateTime fecha = DateTime.Now;
            comentario.fecha = fecha;
            comentario.idPost = idp;
            comentario.idUser = idu;
            _context.Add(comentario);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Comentario Realizado";
            string adm = HttpContext.Session.GetString("_admin").ToString();

            if (adm == "True")
            {
                return RedirectToAction("IndexAdmin", "Home");
            }
            else
            {
                return RedirectToAction(nameof(Index), "Home");
            }

        }

        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
 

            if (id == null || _context.comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idPost,idUser,contenido,fecha")] Comentario comentario, int idp, int idu)
        {


            if (id != comentario.id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(comentario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(comentario.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                TempData["Message"] = "Comentario Modificado";
                return RedirectToAction(nameof(Index), "Home");

            ViewData["idPost"] = new SelectList(_context.post, "id", "id", comentario.idPost);
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", comentario.idUser);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.comentarios
                .Include(c => c.post)
                .Include(c => c.usuario)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.comentarios == null)
            {
                return Problem("Entity set 'MyContext.comentarios'  is null.");
            }
            var comentario = await _context.comentarios.FindAsync(id);
                
            _context.comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Comentario Eliminado";
            return RedirectToAction(nameof(Index), "Home");
        }

        private bool ComentarioExists(int id)
        {
          return (_context.comentarios?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
