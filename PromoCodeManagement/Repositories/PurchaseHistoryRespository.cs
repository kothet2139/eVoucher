using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PromoCodeManagement.Entity;
using PromoCodeManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeManagement.Repositories
{
    public class PurchaseHistoryRespository : IPurchaseHistoryRespository
    {
        private string _connectionString;
        public PurchaseHistoryRespository(IConfiguration iconfiguration, IServiceProvider serviceProvider)
        {
            _connectionString = iconfiguration.GetConnectionString("DefaultConnection");
        }

        public List<Order> GetOrders()
        {
            List<Order> lsOrder = new List<Order>();
            using (var db = new DataContext())
            {
                lsOrder = db.Orders
                                .Where(o => o.generated_status == false && o.payment_status == "success")
                                .ToList();
            }
            return lsOrder;
        }

        public int CheckPromoCode(string promoCode)
        {
            List<PurchaseHistory> purchases = new List<PurchaseHistory>();
            using (var db = new DataContext())
            {
                purchases = db.PurchaseHistories
                                      .Where(p => p.promo_code == promoCode)
                                      .ToList();
            }

            if(purchases != null)
            {
                return purchases.Count();
            }

            return 0;
        }

        public int AddPurchaseHistory(List<PurchaseHistory> purchaseHistories)
        {
            int count = 0;
            using (var db = new DataContext())
            {
                db.PurchaseHistories.AddRange(purchaseHistories);
                count = db.SaveChanges();
            }

            return count;
        }

        public int UpdateGenerateStatusToOrder(Order order)
        {
            int count = 0;
            using (var db = new DataContext())
            {
                db.Entry(order).State = EntityState.Modified;
                count = db.SaveChanges();
            }
            return count;
        }

        #region InLine
        //public IList<Order> GetOrders()
        //{
        //    List<Order> lsOrders = new List<Order>();

        //    using var con = new NpgsqlConnection(_connectionString);
        //    con.Open();
        //    var sql = "SELECT * FROM \"Orders\"";

        //    using var cmd = new NpgsqlCommand(sql, con);
        //    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
        //    adapter.SelectCommand = cmd;

        //    DataSet dataSet = new DataSet();
        //    adapter.Fill(dataSet);
        //    con.Close();

        //    if(dataSet.Tables.Count > 0)
        //    {
        //        if(dataSet.Tables[0].Rows.Count > 0)
        //        {                    
        //            foreach(DataRow row in dataSet.Tables[0].Rows)
        //            {
        //                string id = Convert.ToString(row["id"]);
        //                Order order = new Order()
        //                {
        //                    id =new Guid(id),
        //                    phone_no = Convert.ToString(row["phone_no"]),
        //                    quantity = Convert.ToInt32(row["quantity"]),
        //                    total_amount = Convert.ToDecimal(row["total_amount"]),
        //                    discount_amount = Convert.ToDecimal(row["discount_amount"])
        //                };
        //                lsOrders.Add(order);
        //            }
        //        }
        //    }
        //    return lsOrders;
        //}

        //public int CheckPromoCode(string promoCode)
        //{
        //    using var con = new NpgsqlConnection(_connectionString);
        //    con.Open();
        //    var sql = "SELECT * FROM \"PurchaseHistories\" Where promo_code = '" + promoCode + "'";

        //    using var cmd = new NpgsqlCommand(sql, con);
        //    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
        //    adapter.SelectCommand = cmd;

        //    DataSet dataSet = new DataSet();
        //    adapter.Fill(dataSet);
        //    con.Close();

        //    if (dataSet.Tables.Count > 0)
        //    {
        //        return dataSet.Tables[0].Rows.Count;
        //    }

        //    return 0;
        //}
        #endregion
    }
}
