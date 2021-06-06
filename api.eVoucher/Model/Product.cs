using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Model
{
    public class Product
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
        public DateTime expiry_date { get; set; }
        public decimal amount { get; set; }
        public int quantity { get; set; }
        public bool is_active { get; set; }
        public bool is_onlyme_usage { get; set; }
        public int max_for_me { get; set; }
        public int max_to_gift { get; set; }
        public string image { get; set; }
        public ICollection<PaymentMethod> payment_methods { get; set; }
    }
}
