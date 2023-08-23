using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Repository
{
    public interface IBookRepository
    {
        public Task<List<BookModel>> GetAllBookAsync();
        public Task<BookModel> GetBookByIdAsync(string id);
        public Task<List<BookModel>> GetBookByNameAsync(string NameBook);
        public Task<List<BookModel>> GetByCategoryAsync(string Category);
        public Task<List<BookModel>> GetByPublishingCompanyAsync(string PublishingCompany);
        public Task<string> AddBookAsync(BookModel model);
        public Task UpDateBookAsync(string id, BookModel model);
        public Task DeleteBookAsync(string id);
        public Task ChageNumberSaleAsync(string id, int number);

    }
}
