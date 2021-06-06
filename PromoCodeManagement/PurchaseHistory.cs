using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeManagement
{
    public class PurchaseHistory
    {
        public Guid id { get; set; }
        public string promo_code { get; set; }
        public byte[] qr_code { get; set; }
        public DateTime expiry_date { get; set; }
        public bool is_used { get; set; }
        public Guid orderid { get; set; }
    }
}
