using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApplication1.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryWebApplication1.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly DblibraryContext _context;

        public ArticlesController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Categories", "Index");
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = name;
            var articlesByCategory = _context.Articles.Where(a => a.CategoryId == id).Include(a => a.CategoryNavigation);
            //var dblibraryContext = _context.Articles.Include(a => a.Author).Include(a => a.CategoryNavigation);
            return View(await articlesByCategory.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create(int? categoryId)
        {
            ViewBag.Authors = new SelectList(_context.Users, "UserId", "Username");
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            if (categoryId != null)
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                ViewBag.CategoryId = categoryId;
                ViewBag.CategoryName = category?.Name;
                var article = new Article
                {
                    CategoryId = categoryId,
                    CategoryNavigation = category
                };
                return View(article);
            }
            else
            {
                var article = new Article();
                return View(article);
            }
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,ArticleName,PublishDate,Status,CategoryId")] Article article, int? categoryId)
        {
            if (categoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(categoryId);

                if (category != null)
                {
                    article.CategoryNavigation = category;
                    article.CategoryId = categoryId; // Додайте цей рядок
                    System.Diagnostics.Debug.WriteLine(article.CategoryNavigation.CategoryId);
                    System.Diagnostics.Debug.WriteLine("q");
                }
                else
                {
                    ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                }
            }
            else
            {
                ModelState.AddModelError("CategoryId", "Category is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Якщо модель не валідна, встановлюємо значення для CategoryNavigation, якщо вони є доступними
            System.Diagnostics.Debug.WriteLine(article.CategoryNavigation);
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }
            ViewBag.Authors = new SelectList(_context.Users, "UserId", "Username", article.AuthorId);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", categoryId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,AuthorId,AuthorUsername,ArticleName,CategoryId,Category,PublishDate,Status")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}

