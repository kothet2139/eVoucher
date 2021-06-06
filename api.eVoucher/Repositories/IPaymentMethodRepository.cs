using api.eVoucher.Dtos;
using api.eVoucher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod> Get(Guid id);
        Task<IEnumerable<PaymentMethod>> GetAll();
        Task<IEnumerable<PaymentMethod>> GetAllByProductId(Guid productId);
        Task Delete(Guid id);
        Task Update(Guid id , PaymentMethod paymentMethod);
    }
}
