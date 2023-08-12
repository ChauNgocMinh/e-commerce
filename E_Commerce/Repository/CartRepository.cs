using AutoMapper;
using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_Commerce.Repository
{

    public class CartRepository : ICartRepository
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        private Random _random = new Random();
        public CartRepository(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
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
        public async Task<List<CartItemModel>> ShowCartAsync(string email)
        {

            var ListItem = _context.CartItems
                .Where(CartItems => CartItems.Email == email && CartItems.Status == true)
                .Join(_context.Books,
                    CartItems => CartItems.IdBook,
                    Books => Books.Id,
                    (CartItems, Books) => new CartItemModel
                    {
                        Email = CartItems.Email,
                        Id = CartItems.Id,
                        IdBook = CartItems.IdBook,
                        Name = Books.Namebook,
                        Picture = Books.Picture,
                        Number = CartItems.Number,
                        Status = CartItems.Status,
                        Price = Books.Price,

                    })
                .ToList();

            return ListItem;
        }
        public async Task<CartItemModel> ShowItemCartAsync(string email, string idItem)
        {

            var ListItem = _context.CartItems
                .Where(CartItems => CartItems.Email == email && CartItems.Status == true)
                .Join(_context.Books,
                    CartItems => CartItems.IdBook,
                    Books => Books.Id,
                    (CartItems, Books) => new CartItemModel
                    {
                        Email = CartItems.Email,
                        Id = CartItems.Id,
                        IdBook = CartItems.IdBook,
                        Name = Books.Namebook,
                        Picture = Books.Picture,
                        Number = CartItems.Number,
                        Status = CartItems.Status,
                        Price = Books.Price,

                    })
                .ToList();

            return ListItem.FirstOrDefault();
        }

        public async Task AddItemAsync(CartItemModel model)
        {
            var item = _context.CartItems!.SingleOrDefault(x => x.IdBook == model.IdBook);
            if (item == null)
            {
                var NewItem = _mapper.Map<CartItem>(model);
                await _context.CartItems!.AddAsync(NewItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.Number = item.Number + model.Number;
                await _context.SaveChangesAsync();
                
            }
        }

       
        public async Task RemoveItenAsync(string IdItem)
        {
            var DeleteItem = _context.CartItems!.SingleOrDefault(x => x.Id == IdItem);
            if (DeleteItem != null)
            {
                _context.CartItems!.Remove(DeleteItem);
                _context.SaveChanges();
            }
        }
    }
}
