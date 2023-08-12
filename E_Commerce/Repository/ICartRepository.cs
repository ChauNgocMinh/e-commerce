using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        public Task<List<CartItemModel>> ShowCartAsync(string email);
        public Task<CartItemModel> ShowItemCartAsync(string email, string idItem);

        //public Task<List<SelctItemModel>> SelectItem(SelctItemModel model);
        public Task AddItemAsync(CartItemModel model);
        public Task RemoveItenAsync(string IdItem);

    }
}
