namespace E_Commerce.Models
{
    public class CartItemModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? IdBill { get; set; }
        public string? Email { get; set; } = null!;
        public string? Picture { get; set; }
        public string IdBook { get; set; }
        public double Price { get; set; }
        public int Number { get; set; } 
        public bool? Status { get; set; }
    }
}
