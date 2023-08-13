using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SqlTypes;

namespace E_Commerce.Controllers
{
    //[Route("[Controller]/[action]")]
    public class BooksController : Controller
    {
        public readonly IBookRepository _BookRepo;

        public BooksController(IBookRepository BookRepo)
        {
            _BookRepo = BookRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return View(await _BookRepo.GetAllBookAsync());
        }

        public async Task<ActionResult> AddBook()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBook(BookModel model)
        {
            try
            {
                var NewBook = await _BookRepo.AddBookAsync(model);
                var Book = await _BookRepo.GetBookByIdAsync(NewBook);
                return Book == null ? BadRequest() : Ok(Book);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBookById(string id)
        {
            {
                if (id != null)
                {
                    return View(await _BookRepo.GetBookByIdAsync(id));
                }
                else
                {
                    return BadRequest();
                }
            }
        }
       

    }
}
