using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Repository;
using E_Commerce.Models;
using System.Text;
using E_Commerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System;
using AutoMapper.Execution;

namespace E_Commerce.Controllers
{
    [Route("[Controller]/[action]")]
    [Authorize]
    public class CartController : Controller
    {
        public readonly ICartRepository _cartRepo;
        public readonly IBookRepository _bookRepo;
        //public readonly BookShopContext _context;
        private UserManager<ApplicationUser> _userManager;

        private readonly Random _random = new Random();
        public CartController(ICartRepository cartRepo, IBookRepository bookRepo, UserManager<ApplicationUser> userManager)
        {
            _cartRepo = cartRepo;
            _bookRepo = bookRepo;
            _userManager = userManager;
        }
        [NonAction]
        public string RandomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder idBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                idBuilder.Append(chars[_random.Next(chars.Length)]);
            }
            return idBuilder.ToString();
        }
        [HttpGet]
        public async Task<IActionResult> ShowCart()
        {
            string email = HttpContext.User.Identity.Name;
            return View(await _cartRepo.ShowCartAsync(email));
        }
        [HttpGet]
        public async Task<IActionResult> AddItem(string IDBook, int number)
        {
            try
            {
                string email = HttpContext.User.Identity.Name;
                CartItemModel item = new CartItemModel
                {
                    Id = RandomId(),
                    Email = email.ToString(),
                    IdBook = IDBook,
                    Number = number,
                    Status = true,
                };
                await _cartRepo.AddItemAsync(item);
                return RedirectToAction(nameof(ShowCart));
            }
            catch
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> Buy(List<string> idItem,string IdBook)
        {
            string random = RandomId();
            string email = HttpContext.User.Identity.Name;
            int number = 0;
            var ListSelct = new List<SelctItemModel>();
            if(idItem.Count == 0)
            {
                idItem.Add(random);
                CartItemModel itemCart = new CartItemModel
                {
                    Id = random,
                    Email = email.ToString(),
                    IdBook = IdBook,
                    Number = 1,
                    Status = true,
                };
                await _cartRepo.AddItemAsync(itemCart);
                number = 1;
            }
            foreach (var item in idItem)
            {
                if(number != 0)
                {
                    var itemCart = await _cartRepo.ShowItemSelectAsync(email, IdBook);
                    SelctItemModel model = new SelctItemModel
                    {
                        //Id = RandomId(),
                        IdItem = idItem.ToString(),
                        Number = number,
                        Name = itemCart.Name,
                        Picture = itemCart.Picture,
                        Price = itemCart.Price,
                        Title = itemCart.Price * double.Parse(itemCart.Number.ToString()),
                    };
                    ListSelct.Add(model);
                }
                else
                {
                    var itemCart = await _cartRepo.ShowItemSelectAsync(email, item);
                    SelctItemModel model = new SelctItemModel
                    {
                        //Id = RandomId(),
                        IdItem = itemCart.Id,
                        Number = itemCart.Number,
                        Name = itemCart.Name,
                        Picture = itemCart.Picture,
                        Price = itemCart.Price,
                        Title = itemCart.Price * double.Parse(itemCart.Number.ToString()),
                    };
                    ListSelct.Add(model);
                }
            }
            return View(ListSelct);
        }

       
        [HttpGet]
        public async Task<IActionResult> DeleteItemCart(string id)
        {

            await _cartRepo.RemoveItenAsync(id);
            return RedirectToAction(nameof(ShowCart));
        }
        
    }
}
