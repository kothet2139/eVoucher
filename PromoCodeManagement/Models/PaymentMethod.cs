using System;
using System.Collections.Generic;

#nullable disable

namespace PromoCodeManagement.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
        public Guid Productid { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
