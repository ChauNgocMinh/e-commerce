using System;
using System.Collections.Generic;

namespace E_Commerce.Areas.Identity.Data
{
    public partial class Book
    {
        public Book()
        {
            CartItems = new HashSet<CartItem>();
        }
        public string Id { get; set; } = null!;
        public string? Namebook { get; set; }
        public string? Picture { get; set; }
        public string? PublishingCompany { get; set; }
        public string? Category { get; set; }
        public int Price { get; set; }
        public int Sales { get; set; } 
        public int? Status { get; set; }
        public string? Review { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
