using api.eVoucher.Dtos;
using api.eVoucher.Entity;
using api.eVoucher.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Repositories
{
    public class PurchaseHistoryRespository : IPurchaseHistoryRespository
    {
        private readonly IDataContext _context;
        public PurchaseHistoryRespository(IDataContext context)
        {
            _context = context;
        }
        public async Task Add(PurchaseHistory purchaseHistory)
        {
            purchaseHistory.id = Guid.NewGuid();
            _context.PurchaseHistories.Add(purchaseHistory);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var itemToRemove = await _context.PurchaseHistories.FindAsync(id);

            if (itemToRemove == null)
                throw new NullReferenceException();
            _context.PurchaseHistories.Remove(itemToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<PurchaseHistory> Get(Guid id)
        {
            return await _context.PurchaseHistories.FindAsync(id);
        }

        public async Task<IEnumerable<PurchaseHistory>> GetAll()
        {
            return await _context.PurchaseHistories.ToListAsync();
        }

        public Task<PurchaseHistory> GetPurchaseHistoryByPromoCode(string promoCode)
        {
            return _context.PurchaseHistories
                .FirstOrDefaultAsync(s => s.promo_code == promoCode);
        }

        public async Task<IEnumerable<PurchaseHistory>> GetAllByOrderId(Guid orderId)
        {
            return await _context.PurchaseHistories.Where(ph => ph.orderid == orderId).ToListAsync();
        }

        public async Task Update(Guid id, PurchaseHistory purchaseHistory)
        {
            var itemToUpdate = await _context.Orders.FindAsync(id);
            if (itemToUpdate == null)
                throw new NullReferenceException();
            await _context.SaveChangesAsync();
        }
    }
}
