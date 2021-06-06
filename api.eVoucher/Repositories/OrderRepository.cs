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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataContext _context;
        public OrderRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task Add(Order order)
        {
            order.id = Guid.NewGuid();
            order.tran_date = DateTime.Now;
            //order.payment_date = DateTime.Now;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var itemToRemove = await _context.Orders.FindAsync(id);

            if (itemToRemove == null)
                throw new NullReferenceException();
            _context.Orders.Remove(itemToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<Order> Get(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByUserId(string userId)
        {
            return await _context.Orders.Where(pm => pm.user_id == userId).ToListAsync();
        }

        public async Task Update(Guid id, Order order)
        {
            var itemToUpdate = await _context.Orders.FindAsync(id);
            if (itemToUpdate == null)
                throw new NullReferenceException();
            itemToUpdate.card_number = order.card_number;
            itemToUpdate.cvv = order.cvv;
            itemToUpdate.card_expiry_date = order.card_expiry_date;
            itemToUpdate.payment_status = "success";
            itemToUpdate.tran_status = "success";
            itemToUpdate.payment_date = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
