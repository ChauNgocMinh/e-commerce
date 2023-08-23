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
using PayPal.Api;

namespace E_Commerce.Controllers
{
    [Route("[Controller]/[action]")]
    [Authorize(Roles = "User")]
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
                await _cartRepo.AddItemAsync(item, email);
                return RedirectToAction(nameof(ShowCart));
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> InfoOrder(string idItem, string IdBook, int number)
        {
            try
            {
                List<string> idItemlist = new List<string>();
                List<string> idItemBooklist = new List<string>();
                string email = HttpContext.User.Identity.Name;
                //int number = 0;
                var ListSelct = new List<SelctItemModel>();
                if (idItem == null)
                {
                    string random = RandomId();
                    idItemlist.Add(random);
                    CartItemModel itemCart = new CartItemModel
                    {
                        Id = random,
                        Email = email.ToString(),
                        IdBook = IdBook,
                        Number = number,
                        Status = true,
                        Price = 0,
                    };
                    await _cartRepo.AddItemAsync(itemCart, email);
                }
                else
                {
                    idItemlist = new List<string>(idItem.Split(','));
                    idItemBooklist = new List<string>(IdBook.Split(','));
                }
                foreach (var item in idItemlist)
                {
                    //var model = _cartRepo.
                    if (idItem == null)
                    {
                        var itemCart = await _cartRepo.ShowItemSelectAsync(email, IdBook);
                        SelctItemModel model = new SelctItemModel
                        {
                            Id = IdBook,
                            //Id = RandomId(),
                            IdItem = itemCart.Id,
                            Number = number,
                            NameBook = itemCart.Name,
                            Picture = itemCart.Picture,
                            Price = itemCart.Price,
                            Title = itemCart.Price,
                        };
                        ListSelct.Add(model);
                    }
                    else
                    {
                        var itemCart = await _cartRepo.ShowItemSelecByIDAsync(email, item);
                        SelctItemModel model = new SelctItemModel
                        {
                            Id = itemCart.IdBook,
                            //Id = RandomId(),
                            IdItem = itemCart.Id,
                            Number = itemCart.Number,
                            NameBook = itemCart.Name,
                            Picture = itemCart.Picture,
                            Price = itemCart.Price,
                            Title = itemCart.Price * double.Parse(itemCart.Number.ToString()),
                        };
                        ListSelct.Add(model);
                    }
                }
                InfoOrderModel infoOrder = new InfoOrderModel()
                {
                    selctItemModels = ListSelct,
                };
                return View(infoOrder);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buy(string idItem, string IdBook)
        {
            try
            {
                List<string> idItemlist = new List<string>();

                string email = HttpContext.User.Identity.Name;
                int number = 0;
                var ListSelct = new List<SelctItemModel>();
                if (idItem == null)
                {
                    string random = RandomId();
                    idItemlist.Add(random);
                    CartItemModel itemCart = new CartItemModel
                    {
                        Id = random,
                        Email = email.ToString(),
                        IdBook = IdBook,
                        Number = 1,
                        Status = true,
                    };
                    await _cartRepo.AddItemAsync(itemCart, email);
                    number = 1;
                }
                else
                {
                    idItemlist = new List<string>(idItem.Split(','));
                }
                foreach (var item in idItemlist)
                {
                    if (number != 0)
                    {
                        var itemCart = await _cartRepo.ShowItemSelectAsync(email, IdBook);
                        SelctItemModel model = new SelctItemModel
                        {
                            //Id = RandomId(),
                            IdItem = itemCart.Id,
                            Number = number,
                            NameBook = itemCart.Name,
                            Picture = itemCart.Picture,
                            Price = itemCart.Price,
                            Title = itemCart.Price * double.Parse(itemCart.Number.ToString()),
                        };
                        ListSelct.Add(model);
                    }
                    else
                    {
                        var itemCart = await _cartRepo.ShowItemSelecByIDAsync(email, item);
                        SelctItemModel model = new SelctItemModel
                        {
                            //Id = RandomId(),
                            IdItem = itemCart.Id,
                            Number = itemCart.Number,
                            NameBook = itemCart.Name,
                            Picture = itemCart.Picture,
                            Price = itemCart.Price,
                            Title = itemCart.Price * double.Parse(itemCart.Number.ToString()),
                        };
                        ListSelct.Add(model);
                    }
                }
                InfoOrderModel infoOrder = new InfoOrderModel()
                {
                    selctItemModels = ListSelct,
                };
                return View(infoOrder);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItemCart(string id)
        {

            await _cartRepo.RemoveItenAsync(id);
            return RedirectToAction(nameof(ShowCart));
        }
        
        public async Task<IActionResult> ChangeNumber(string idAndNumber)
        {
            string email = HttpContext.User.Identity.Name;
            string[] pairs = idAndNumber.Split(',');

            List<ItemModel> itemList = new List<ItemModel>();

            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('.');

                if (parts.Length == 2)
                {
                    itemList.Add(new ItemModel { Id = parts[0], Number = parts[1] });
                }
            }


            foreach (ItemModel item in itemList)
            {
                await _cartRepo.ChangeNumberAsync(email, item.Id, int.Parse(item.Number));
            }
            return RedirectToAction(nameof(ShowCart));
        }
    }
    public class ItemModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
    }
}
