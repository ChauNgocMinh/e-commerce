using System;
using System.Collections.Generic;

namespace E_Commerce.Areas.Identity.Data
{
    public partial class CartItem
    {
        public CartItem()
        {
            Bills = new HashSet<Bill>();
        }

        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string IdBook { get; set; } = null!;
        public int Number { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public virtual Book IdBookNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
