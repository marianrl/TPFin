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
    public class TagsController : Controller
    {
        private readonly MyContext _context;

        public TagsController(MyContext context)
        {
            _context = context;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
              return _context.tags != null ? 
                          View(await _context.tags.ToListAsync()) :
                          Problem("Entity set 'MyContext.tags'  is null.");
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tags == null)
            {
                return NotFound();
            }

            var tag = await _context.tags
                .FirstOrDefaultAsync(m => m.id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,palabra")] Tag tag)
        {
            if(tag == null)
            {
                string frase = TempData["tags"].ToString();
                List<string> palabras = frase.Split(' ').ToList();
                foreach(string a in palabras)
                {
                    if (a != _context.tags.Where(x => x.palabra == a).ToString())
                    {
                        tag.palabra = a;
                        _context.Add(tag);                        
                        await _context.SaveChangesAsync();
                    }
                }
                IEnumerable<Tag> tags = _context.tags.Where(x => x.id == tag.id);
                TempData["tags2"] = tags;
            }
            return RedirectToAction(nameof(Create),"PostsTags");
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tags == null)
            {
                return NotFound();
            }

            var tag = await _context.tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,palabra")] Tag tag)
        {
            if (id != tag.id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tags == null)
            {
                return NotFound();
            }

            var tag = await _context.tags
                .FirstOrDefaultAsync(m => m.id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tags == null)
            {
                return Problem("Entity set 'MyContext.tags'  is null.");
            }
            var tag = await _context.tags.FindAsync(id);
            if (tag != null)
            {
                _context.tags.Remove(tag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
          return (_context.tags?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
