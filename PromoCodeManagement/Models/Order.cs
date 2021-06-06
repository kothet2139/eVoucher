using System;
using System.Collections.Generic;

#nullable disable

namespace PromoCodeManagement.Models
{
    public partial class Order
    {
        public Order()
        {
            PurchaseHistories = new HashSet<PurchaseHistory>();
        }

        public Guid Id { get; set; }
        public Guid Productid { get; set; }
        public string CardNumber { get; set; }
        public Guid PaymentMethodid { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string TranId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int Quantity { get; set; }
        public string TranStatus { get; set; }
        public string PaymentStatus { get; set; }
        public bool GeneratedStatus { get; set; }
        public DateTime TranDate { get; set; }
        public string CardExpiryDate { get; set; }
        public string Cvv { get; set; }
        public DateTime PaymentDate { get; set; }
        public string UserId { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; }
    }
}
