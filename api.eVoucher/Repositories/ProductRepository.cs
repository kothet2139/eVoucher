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
    public class ProductRepository : IProductRepository
    {
        private readonly IDataContext _context;
        public ProductRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task Add(Product product)
        {
            product.id = Guid.NewGuid();
            product.created_date = DateTime.Now;
            product.modified_date = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var paymentItemToRemove = await _context.PaymentMethod.Where(pm => pm.Product.id == id).ToListAsync();
            var itemToRemove = await _context.Products.FindAsync(id);

            if (itemToRemove == null)
                throw new NullReferenceException();
            if(paymentItemToRemove != null)
            {
                _context.PaymentMethod.RemoveRange(paymentItemToRemove);
            }
            _context.Products.Remove(itemToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<Product> Get(Guid id)
        {
            return await _context.Products.Include(p=>p.payment_methods).FirstOrDefaultAsync(pp => pp.id == id);
        }

        public async Task<Product> GetActive(Guid id)
        {
            return await _context.Products.Where(p => p.is_active && p.id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(p => p.payment_methods).ToListAsync();
        }

        public async Task Update(Guid id, Product product)
        {
            var itemToUpdate = await _context.Products.FindAsync(id);
            if (itemToUpdate == null)
                throw new NullReferenceException();
            if (!string.IsNullOrEmpty(product.title))
            {
                itemToUpdate.title = product.title;
            }
            if (!string.IsNullOrEmpty(product.description))
            {
                itemToUpdate.description = product.description;
            }
            itemToUpdate.modified_date = DateTime.Now;

            if (product.amount > 0)
            {
                itemToUpdate.amount = product.amount;
            }
            if (product.quantity > 0)
            {
                itemToUpdate.quantity = product.quantity;
            }
            if (product.is_active != itemToUpdate.is_active)
            {
                itemToUpdate.is_active = product.is_active;
            }
            if (product.is_onlyme_usage != itemToUpdate.is_onlyme_usage)
            {
                itemToUpdate.is_onlyme_usage = product.is_onlyme_usage;
            }
            if (product.max_for_me > 0)
            {
                itemToUpdate.max_for_me = product.max_for_me;
            }
            if (product.max_to_gift > 0)
            {
                itemToUpdate.max_to_gift = product.max_to_gift;
            }
            if (!string.IsNullOrEmpty(product.image))
            {
                itemToUpdate.image = product.image;
            }
            await _context.SaveChangesAsync();
        }
    }
}
