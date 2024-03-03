using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApplication1.Models;

namespace LibraryWebApplication1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DblibraryContext _context;

        public CategoriesController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            // Отримуємо статті, що відносяться до конкретної категорії
            var articlesInCategory = await _context.Articles
                .Where(a => a.CategoryId == id)
                .ToListAsync();

            // Передаємо список статей у вигляд для відображення
            ViewBag.Articles = articlesInCategory;

            return View(category);
        }

        // GET: Categories/Create
        // GET: Categories/Create
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                int maxCategoryId = _context.Categories.Max(c => (int?)c.CategoryId) ?? 0;
                category.CategoryId = maxCategoryId + 1;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var articlesToDelete = _context.Articles.Where(a => a.CategoryId == category.CategoryId);
            _context.Articles.RemoveRange(articlesToDelete);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RelatedArticles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articlesInCategory = await _context.Articles
                .Where(a => a.CategoryId == id)
                .ToListAsync();

            var categoryName = _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => c.Name)
                .FirstOrDefault();

            ViewBag.CategoryName = categoryName;

            return View("RelatedArticles", articlesInCategory);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}


