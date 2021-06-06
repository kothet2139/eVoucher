using api.eVoucher.Dtos;
using api.eVoucher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Repositories
{
    public interface IPurchaseHistoryRespository
    {
        Task Add(PurchaseHistory order);
        Task<PurchaseHistory> Get(Guid id);
        Task<IEnumerable<PurchaseHistory>> GetAll();
        Task<PurchaseHistory> GetPurchaseHistoryByPromoCode(string promoCode);
        Task<IEnumerable<PurchaseHistory>> GetAllByOrderId(Guid orderId);
        Task Delete(Guid id);
        Task Update(Guid id , PurchaseHistory purchaseHistory);
    }
}
