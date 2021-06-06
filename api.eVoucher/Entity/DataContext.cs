using api.eVoucher.Authentication;
using api.eVoucher.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.eVoucher.Entity
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
