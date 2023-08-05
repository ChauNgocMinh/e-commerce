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
    [Route("Controller/[action]")]
    [ApiController]
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

        [HttpGet]
        public async Task<ActionResult> GetBookById(string id)
        {
            {
                if (id != null)
                {
                    return Ok(await _BookRepo.GetBookByIdAsync(id));
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetBookByName(string NameBook)
        {
            if (NameBook != null)
            {
                return Ok(await _BookRepo.GetBookByNameAsync(NameBook));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetBookByCategory(string Category)
        {
            if (Category != null)
            {
                return Ok(await _BookRepo.GetByCategoryAsync(Category));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetBookByPublishingCompany(string PublishingCompany)
        {
            if (PublishingCompany != null)
            {
                return Ok(await _BookRepo.GetByPublishingCompanyAsync(PublishingCompany));
            }
            else
            {
                return BadRequest();
            }
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

        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateBook(string id, BookModel model)
        {
            if (id == model.Id)
            {
                await _BookRepo.UpDateBookAsync(id, model);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBook(string id)
        {
            await _BookRepo.DeleteBookAsync(id);
            return Ok();
        }

        }
    }
