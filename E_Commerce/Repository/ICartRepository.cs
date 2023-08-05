using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        public Task<List<CartItemModel>> ShowCartAsync(string email);
        public Task<List<BillModel>> BillHistoryAsync();
        public Task<List<BillModel>> GetItemCartByDateAsync(DateTime Time);
        public Task<BillModel> GetCartByIdAsync(string Id);
        public Task<string> AddItemAsync(CartItemModel model);
        public Task EditNumberItemAsync(string IdItem, int Number);
        public Task RemoveItenAsync(string IdItem);
        public Task<BillModel> BuyAsync(string IdCart, BillModel model);

    }
}
