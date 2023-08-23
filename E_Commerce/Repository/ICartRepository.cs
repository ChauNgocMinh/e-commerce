using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        public Task<List<CartItemModel>> ShowCartAsync(string email);
        public Task<CartItemModel> ShowItemSelectAsync(string email, string idItem);
        public Task<CartItem> UpdateCartItemAsync(string email, string Id, string IdBill);
        //public Task<List<SelctItemModel>> SelectItem(SelctItemModel model);
        public Task ChangeNumberAsync(string email, string Id, int number);
        public Task AddItemAsync(CartItemModel model, string email);
        public Task RemoveItenAsync(string IdItem);
        public Task<CartItemModel> ShowItemSelecByIDAsync(string email, string idItem);
    }
}
