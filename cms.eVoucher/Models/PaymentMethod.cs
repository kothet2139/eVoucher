using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cms.eVoucher.Model
{
    public class PaymentMethod
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public decimal discount { get; set; }
        public Product Product { get; set; }
        public Guid Productid { get; set; }
    }
}
