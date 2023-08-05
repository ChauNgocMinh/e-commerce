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

        public async Task<string> AddItemAsync(CartItemModel model)
        {
            var NewItem = _mapper.Map<CartItem>(model);
            await _context.CartItems!.AddAsync(NewItem);
            await _context.SaveChangesAsync();
            return NewItem.Id;
        }

        public async Task<BillModel> BuyAsync(string IdCart, BillModel model)
        {
            var Item = _context.CartItems!.SingleOrDefault(x => x.Id == IdCart);
            Item.Status = false;
            var History = _mapper.Map<Bill>(model);
            await _context.AddAsync(History);
            await _context.SaveChangesAsync();
            return model;
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

        public async Task<List<BillModel>> BillHistoryAsync()
        {
            var ListCart = await _context.Bills!.ToListAsync();
            return _mapper.Map<List<BillModel>>(ListCart);
        }

        public async Task<BillModel> GetCartByIdAsync(string Id)
        {
            var Bill = await _context.Bills!.SingleOrDefaultAsync(b => b.Id == Id);
            return _mapper.Map<BillModel>(Bill);
        }

        public async Task EditNumberItemAsync(string IdItem, int Number)
        {
            var EditItem = _context.CartItems!.SingleOrDefault(x => x.Id == IdItem);
            var Book = _context.Books!.SingleOrDefault(x => x.Id == EditItem.Id);

            if (EditItem != null)
            {
                EditItem.Number = Number;
                /*                EditItem.TotalItem = Book.Price * Number;
                */
                if (EditItem.Number == 0)
                {
                    await RemoveItenAsync(IdItem);
                }
                _context.SaveChanges();
            }
        }

        public Task<List<BillModel>> GetItemCartByDateAsync(DateTime Time)
        {
            throw new NotImplementedException();
        }
    }
}
