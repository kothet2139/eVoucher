using api.eVoucher.Dtos;
using api.eVoucher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Repositories
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<Order> Get(Guid id);
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetAllByUserId(string userId);
        Task Delete(Guid id);
        Task Update(Guid id , Order order);
    }
}
