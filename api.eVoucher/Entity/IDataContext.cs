using api.eVoucher.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace api.eVoucher.Entity
{
    public interface IDataContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<PaymentMethod> PaymentMethod { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
