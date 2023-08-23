using System;
using System.Collections.Generic;

namespace E_Commerce.Areas.Identity.Data
{
    public partial class Bill
    {
        public Bill()
        {
            CartItems = new HashSet<CartItem>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public DateTime BuyingDate { get; set; }
        public int Total { get; set; }
        public int Pay { get; set; }
        public string Address { get; set; } = null!;
        public bool Status { get; set; }
        public string? Note { get; set; }
        public string PaymentMethods { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
