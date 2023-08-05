namespace E_Commerce.Models
{
    public class BillModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; }

        public string IdCartItem { get; set; } = null!;

        public DateTime BuyingDate { get; set; }
    }
}
