using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface IBillRepository
    {
        public Task<BillModel> CreateBillAsync(BillModel model);
        public Task UpdateBillAsync(string id);
        public Task<List<InfoOrderModel>> GetAllBillByEmailAsync(string email);
        public Task<List<BillModel>> GetAllBill();
    }
}
