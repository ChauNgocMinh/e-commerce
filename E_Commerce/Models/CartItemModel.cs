namespace E_Commerce.Models
{
    public class CartItemModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; } = null!;

        public string? Email { get; set; } = null!;
        public string? Picture { get; set; }
        public string? IdBook { get; set; }
        public double? Price { get; set; }

        public int? Number { get; set; } = null!;

        public bool? Status { get; set; }

        public DateTime Date { get; set; }
    }
}
