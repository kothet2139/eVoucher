using System;
using System.Collections.Generic;

#nullable disable

namespace PromoCodeManagement.Models
{
    public partial class PurchaseHistory
    {
        public Guid Id { get; set; }
        public string PromoCode { get; set; }
        public byte[] QrCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public Guid Orderid { get; set; }

        public virtual Order Order { get; set; }
    }
}
