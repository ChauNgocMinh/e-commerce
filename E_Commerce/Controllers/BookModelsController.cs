using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;

namespace E_Commerce.Controllers
{
    public class BookModelsController : Controller
    {
        private readonly BookShopContext _context;

        public BookModelsController(BookShopContext context)
        {
            _context = context;
        }

        // GET: BookModels
        public async Task<IActionResult> Index()
        {
              return _context.BookModel != null ? 
                          View(await _context.BookModel.ToListAsync()) :
                          Problem("Entity set 'BookShopContext.BookModel'  is null.");
        }

        // GET: BookModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: BookModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, string namebook, string picture, string publishingCompany, string category, int price, int status, string review)
        {
            BookModel model = new BookModel()
            {
                Id = id,
                Namebook = namebook,
                Picture = picture,
                PublishingCompany = publishingCompany,
                Category = category,
                Price = price,
                Sales = 0,
                Status = status,
                Review = review,
            };
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BookModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            return View(bookModel);
        }

        // POST: BookModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Namebook,Picture,PublishingCompany,Category,Price,Status,Review")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModelExists(bookModel.Id))
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
            return View(bookModel);
        }

        // GET: BookModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: BookModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BookModel == null)
            {
                return Problem("Entity set 'BookShopContext.BookModel'  is null.");
            }
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                _context.BookModel.Remove(bookModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(string id)
        {
          return (_context.BookModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
