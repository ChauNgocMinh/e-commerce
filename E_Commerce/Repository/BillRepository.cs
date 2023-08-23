using AutoMapper;
using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;

namespace E_Commerce.Repository
{
    public class BillRepository : IBillRepository
    {
        public readonly BookShopContext _context;
        public readonly IMapper _mapper;
        public BillRepository(IMapper mapper, BookShopContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<BillModel> CreateBillAsync(BillModel model)
        {
            var NewItem = _mapper.Map<Bill>(model);
            await _context.Bills!.AddAsync(NewItem);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<InfoOrderModel>> GetAllBillByEmailAsync(string email)
        {
            var bookTable = await _context.Books!.ToListAsync();
            var cartItemTable = await _context.CartItems!.Where(x => x.Email == email).ToListAsync();
            var billTable = await _context.Bills!.Where(x => x.Email == email).ToListAsync();

            var joinedData = from cartItem in cartItemTable
                             join book in bookTable on cartItem.IdBook equals book.Id
                             join bill in billTable on cartItem.IdBill equals bill.Id
                             group new { cartItem, book } by bill into groupedData
                             select new InfoOrderModel
                             {
                                 Id = groupedData.Key.Id,
                                 Name = groupedData.Key.Name,
                                 Email = groupedData.Key.Email,
                                 BuyingDate = groupedData.Key.BuyingDate,
                                 Total = groupedData.Key.Total,
                                 Pay = groupedData.Key.Pay,
                                 Address = groupedData.Key.Address,
                                 Status = groupedData.Key.Status,
                                 Note = groupedData.Key.Note,
                                 PaymentMethods = groupedData.Key.PaymentMethods,
                                 Phone = groupedData.Key.Phone,
                                 selctItemModels = groupedData.Select(item => new SelctItemModel
                                 {
                                    
                                         NameBook = item.book.Namebook,
                                         Price = item.book.Price,
                                         Number = item.cartItem.Number,
                                         Picture = item.book.Picture,
                                         Title = item.book.Price * item.cartItem.Number,
                                     
                                 }).ToList(),

                             };
            return joinedData.ToList();
        }

        public async Task<List<BillModel>> GetAllBill()
        {
            var ListItem = await _context.Bills!.ToListAsync();
            //return ListBook;
            return _mapper.Map<List<BillModel>>(ListItem);
        }
        
        public async Task UpdateBillAsync(string id)
        {
            var a = await _context.Bills!.FirstOrDefaultAsync(x => x.Id == id);
            a.Status = true;
            await _context.SaveChangesAsync();
        }
    }
}
