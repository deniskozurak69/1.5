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
    public class UsersController : Controller
    {
        private readonly DblibraryContext _context;

        public UsersController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                int maxUserId = _context.Users.Max(c => (int?)c.UserId) ?? 0;
                user.UserId = maxUserId + 1;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                var requestsToDelete = _context.SearchRequests.Where(a => a.UserId == user.UserId);
                _context.SearchRequests.RemoveRange(requestsToDelete);
                var articlesToDelete = _context.Articles.Where(a => a.AuthorId == user.UserId);
                _context.Articles.RemoveRange(articlesToDelete);

                var commentsToDelete = _context.Comments
                    .Where(c => c.AuthorUsername == user.Username || articlesToDelete.Any(a => a.ArticleId == c.ArticleId));
                _context.Comments.RemoveRange(commentsToDelete);

                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RelatedArticles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articlesOfUser = await _context.Articles
                .Where(a => a.AuthorId == id)
                .ToListAsync();

            var userName = _context.Users
                .Where(c => c.UserId == id)
                .Select(c => c.Username)
                .FirstOrDefault();

            ViewBag.Username = userName;

            return View("RelatedArticles", articlesOfUser);
        }
        public async Task<IActionResult> RelatedComments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentsOfUser = await _context.Comments
                .Where(a => a.AuthorId == id)
                .ToListAsync();

            var userName = _context.Users
                .Where(c => c.UserId == id)
                .Select(c => c.Username)
                .FirstOrDefault();

            ViewBag.Username = userName;

            return View("RelatedComments", commentsOfUser);
        }

        public async Task<IActionResult> RelatedSearchRequests(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestsOfUser = await _context.SearchRequests
                .Where(a => a.UserId == id)
                .ToListAsync();

            var userName = _context.Users
                .Where(c => c.UserId == id)
                .Select(c => c.Username)
                .FirstOrDefault();

            ViewBag.Username = userName;

            return View("RelatedSearchRequests", requestsOfUser);
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
