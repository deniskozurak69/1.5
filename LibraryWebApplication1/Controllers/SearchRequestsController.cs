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
    public class SearchRequestsController : Controller
    {
        private readonly DblibraryContext _context;

        public SearchRequestsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: SearchRequests
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.SearchRequests.Include(s => s.User);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: SearchRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchRequest = await _context.SearchRequests
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SearchRequestId == id);
            if (searchRequest == null)
            {
                return NotFound();
            }

            return View(searchRequest);
        }

        // GET: SearchRequests/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username");
            return View();
        }

        // POST: SearchRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SearchRequestId,UserId,RequestedName,RequestedCategory,RequestedAuthor,RequestedDate")] SearchRequest searchRequest)
        {
            if (ModelState.IsValid)
            {
                int maxRequestId = _context.SearchRequests.Max(c => (int?)c.SearchRequestId) ?? 0;
                searchRequest.SearchRequestId = maxRequestId + 1;
                _context.Add(searchRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", searchRequest.UserId);
            return View(searchRequest);
        }

        // GET: SearchRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchRequest = await _context.SearchRequests.FindAsync(id);
            if (searchRequest == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Password", searchRequest.UserId);
            return View(searchRequest);
        }

        // POST: SearchRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SearchRequestId,UserId,RequestedName,RequestedCategory,RequestedAuthor,RequestedDate")] SearchRequest searchRequest)
        {
            if (id != searchRequest.SearchRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(searchRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SearchRequestExists(searchRequest.SearchRequestId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Password", searchRequest.UserId);
            return View(searchRequest);
        }

        // GET: SearchRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchRequest = await _context.SearchRequests
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SearchRequestId == id);
            if (searchRequest == null)
            {
                return NotFound();
            }

            return View(searchRequest);
        }

        // POST: SearchRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var searchRequest = await _context.SearchRequests.FindAsync(id);
            if (searchRequest != null)
            {
                _context.SearchRequests.Remove(searchRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SearchRequestExists(int id)
        {
            return _context.SearchRequests.Any(e => e.SearchRequestId == id);
        }
    }
}
