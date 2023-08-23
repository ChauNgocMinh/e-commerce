using PayPal.Api;
namespace E_Commerce.Repository
{
    public interface IPayPalReposytory
    {
        Task<Payment> createOrderAsync(decimal amount, string returnUrl, string cancelUrl);
    }
}
