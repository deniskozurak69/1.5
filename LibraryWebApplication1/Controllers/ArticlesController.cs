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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
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
                    Category = category?.Name,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,AuthorUsername,ArticleName,PublishDate,CategoryId,Category")] Article article, int? categoryId)
        {
            if (categoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(categoryId);

                if (category != null)
                {
                    article.CategoryNavigation = category;
                    article.Category = category.Name;
                    article.CategoryId = categoryId;
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

            if (article.AuthorId.HasValue)
            {
                var author = await _context.Users.FindAsync(article.AuthorId);
                article.Author = author;
                article.AuthorUsername = author?.Username;
            }

            if (ModelState.IsValid)
            {
                int maxArticleId = _context.Articles.Max(c => (int?)c.ArticleId) ?? 0;
                article.ArticleId = maxArticleId + 1;
                _context.Add(article);
                //category.Articles.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            System.Diagnostics.Debug.WriteLine(article.CategoryNavigation);
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }

            ViewBag.Authors = new SelectList(_context.Users, "UserId", "Username", article.AuthorId);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);

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

            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Username", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);
            ViewData["AuthorName"] = _context.Users.FirstOrDefault(u => u.UserId == article.AuthorId)?.Username;
            ViewData["CategoryName"] = _context.Categories.FirstOrDefault(c => c.CategoryId == article.CategoryId)?.Name;

            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,AuthorId,ArticleName,CategoryId,PublishDate")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    article.AuthorUsername = _context.Users.FirstOrDefault(u => u.UserId == article.AuthorId)?.Username;
                    article.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == article.CategoryId)?.Name;
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
            if (article == null)
            {
                return NotFound();    
            }
            var commentsToDelete = _context.Comments.Where(a => a.ArticleId == article.ArticleId);
            _context.Comments.RemoveRange(commentsToDelete);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RelatedComments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentsUnderArticle = await _context.Comments
                .Where(a => a.ArticleId == id)
                .ToListAsync();

            var articleName = _context.Articles
                .Where(c => c.ArticleId == id)
                .Select(c => c.ArticleName)
                .FirstOrDefault();

            ViewBag.ArticleName = articleName;

            return View("RelatedComments", commentsUnderArticle);
        }
        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}

