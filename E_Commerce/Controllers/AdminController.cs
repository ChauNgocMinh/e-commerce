using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Areas.Identity.Data;
using E_Commerce.Repository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[Controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IBillRepository _billRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IAccountRepository _accRepo;


        public AdminController(IAccountRepository accRepo, IBillRepository billRepo, IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
            _billRepo = billRepo;
            _accRepo = accRepo;
    }
    
        public async Task<IActionResult> BillControl()
        {
            return View(await _billRepo.GetAllBill());
        }

        public async Task<IActionResult> BookControl()
        {
            return View(await _bookRepo.GetAllBookAsync());
        }
        public async Task<IActionResult> AccControl()
        {
            return View(await _accRepo.ShowUserAsync());
        }
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            await _accRepo.GetUesrByEmailAsync(email);
            return View();
        }
       
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(string Id, string Namebook, string Picture, string PublishingCompany, string Category, int Price, string Review)
        {
            BookModel model = new BookModel()
            {
                Id = Id,
                Namebook = Namebook,
                Picture = Picture,
                PublishingCompany = PublishingCompany,
                Category = Category,
                Price = Price,
                Sales = 0,
                Status = 1,
                Review = Review,
            };
            try
            {
                var NewBook = await _bookRepo.AddBookAsync(model);
                var Book = await _bookRepo.GetBookByIdAsync(NewBook);
                return View(Book == null ? BadRequest() : Book);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
