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
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly IDataContext _context;
        public PaymentMethodRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid id)
        {
            var itemToRemove = await _context.PaymentMethod.FindAsync(id);

            if (itemToRemove == null)
                throw new NullReferenceException();
            _context.PaymentMethod.Remove(itemToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<PaymentMethod> Get(Guid id)
        {
            return await _context.PaymentMethod.FindAsync(id);
        }

        public async Task<IEnumerable<PaymentMethod>> GetAll()
        {
            return await _context.PaymentMethod.ToListAsync();
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllByProductId(Guid productId)
        {
            return await _context.PaymentMethod.Where(pm => pm.Product.id == productId).ToListAsync();
        }

        public async Task Update(Guid id, PaymentMethod paymentMethod)
        {
            var itemToUpdate = await _context.PaymentMethod.FindAsync(id);
            if (itemToUpdate == null)
                throw new NullReferenceException();
            itemToUpdate.name = paymentMethod.name;
            itemToUpdate.discount = paymentMethod.discount;
            await _context.SaveChangesAsync();
        }
    }
}
