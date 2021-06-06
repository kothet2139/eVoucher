using api.eVoucher.Dtos;
using api.eVoucher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Get(Guid id);
        Task<Product> GetActive(Guid id);
        Task<IEnumerable<Product>> GetAll();
        Task Add(Product product);
        Task Delete(Guid id);
        Task Update(Guid id , Product product);
    }
}
