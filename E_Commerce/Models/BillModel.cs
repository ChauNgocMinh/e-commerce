namespace E_Commerce.Models
{
    public class BillModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime BuyingDate { get; set; }
        public int Total { get; set; }
        public int Pay { get; set; }
        public string Address { get; set; } = null!;
        public bool Status { get; set; }
    }
}
