using System;
using System.Collections.Generic;

#nullable disable

namespace PromoCodeManagement.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
            PaymentMethods = new HashSet<PaymentMethod>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnlymeUsage { get; set; }
        public int MaxForMe { get; set; }
        public int MaxToGift { get; set; }
        public string ModifiedBy { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
