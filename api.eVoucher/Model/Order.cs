using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Model
{
    public class Order
    {
        public Guid id { get; set; }
        public Product product { get; set; }
        public Guid productid { get; set; }
        public string user_id { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string card_expiry_date { get; set; }
        public PaymentMethod payment_method { get; set; }
        public Guid payment_methodid { get; set; }
        public string name { get; set; }
        public string phone_no { get; set; }
        public string tran_id { get; set; }
        public decimal total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public int quantity { get; set; }
        public string tran_status { get; set; }
        public string payment_status { get; set; }
        public bool generated_status { get; set; }
        public DateTime tran_date { get; set; }
        public DateTime payment_date { get; set; }
    }
}
