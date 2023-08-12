using System;
using System.Collections.Generic;

namespace E_Commerce.Areas.Identity.Data
{
    public partial class CartItem
    {
        public string Id { get; set; } = null!;
        public string? IdBill { get; set; }
        public string Email { get; set; } = null!;
        public string IdBook { get; set; } = null!;
        public int Number { get; set; }
        public bool Status { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Book IdBookNavigation { get; set; } = null!;
    }
}
