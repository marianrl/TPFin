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
    public class PostsController : Controller
    {
        private readonly MyContext _context;

        public PostsController(MyContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var myContext = _context.post.Include(p => p.user);
            return View(await myContext.ToListAsync());
        }




        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idUser,contenido,fecha")] Post post, string tags)
        {
            var id = HttpContext.Session.GetInt32("_id");
            DateTime fecha = DateTime.Now;
            post.fecha = fecha;
            if(id != null) { post.idUser = (int)id; }

            if(tags != null)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();

                List<string> palabras = tags.Split(' ').ToList();
                
                foreach (string a in palabras)
                {
                    Tag tag = new Tag();

                    if (_context.tags.Where(x => x.palabra == a).ToArray().Length > 0)
                    {
                        tag = _context.tags.Where(x => x.palabra == a).First();
                    }
                    else
                    {
                        tag.palabra = a;
                        _context.Add(tag);
                        await _context.SaveChangesAsync();
                    }

                    PostsTags postsTags = new PostsTags();
                    postsTags.idTag = tag.id;
                    postsTags.idPost = post.id;
                    _context.Add(postsTags);
                    await _context.SaveChangesAsync();
                }
            }
            else 
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Post Creado";
            }

            TempData["Message"] = "Post Creado";
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", post.idUser);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idUser,contenido,fecha")] Post post)
        {
            if (id != post.id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Message"] = "Post Modificado";
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.post == null)
            {
                return Problem("Entity set 'MyContext.post'  is null.");
            }
            var post = await _context.post.FindAsync(id);
            if (post != null)
            {
                _context.post.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            TempData["Message"] = "Post Eliminado";
            return RedirectToAction(nameof(Index),"Home");
        }

        private bool PostExists(int id)
        {
          return (_context.post?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
