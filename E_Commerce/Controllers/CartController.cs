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

namespace E_Commerce.Controllers
{
    [Route("Controller/[action]")]
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
        /*  [HttpPost]
          public async Task<ActionResult> AddItem(string Id, int number)
          {
              try
              {
                  var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
                  var book = await _bookRepo.GetBookByIdAsync(Id);
                  CartItemModel item = new CartItemModel
                  {
                      Id = RandomId(),
                      Email = email.ToString(),
                      IdBook = Id,
                      Number = number,
                      TotalItem = number * book.Price,
                      Status = true,
                      Date = DateTime.Now,
                  };
                  await _cartRepo.AddItemAsync(item);
                  return Ok();
              }
              catch
              {
                  return NotFound();
              }

          }
          [HttpPost]*/
        public async Task<ActionResult> Buy(string Id)
        {
            try
            {
                var Item = await _cartRepo.GetCartByIdAsync(Id);
                var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

                var NewBill = new BillModel
                {
                    Id = RandomId(),
                    Email = email,
                    IdCartItem = Id,
                    BuyingDate = DateTime.Now,
                };
                await _cartRepo.BuyAsync(Id, NewBill);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<ActionResult> BillHistory()
        {
            await _cartRepo.BillHistoryAsync();
            return Ok();
        }
        [HttpDelete("Id")]
        public async Task<ActionResult> DeleleItemCart(string Id)
        {
            await _cartRepo.RemoveItenAsync(Id);
            var result = $"You clicked the button with ID: and it will be deleted.";
            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult> EditNumberItemCart(string IdItem, int number)
        {
            try
            {
                await _cartRepo.EditNumberItemAsync(IdItem, number);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
