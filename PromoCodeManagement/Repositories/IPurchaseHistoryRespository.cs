using PromoCodeManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeManagement.Repositories
{
    public interface IPurchaseHistoryRespository
    {
        List<Order> GetOrders();
        int CheckPromoCode(string promoCode);
        int AddPurchaseHistory(List<PurchaseHistory> purchaseHistories);
        int UpdateGenerateStatusToOrder(Order order);
    }
}
