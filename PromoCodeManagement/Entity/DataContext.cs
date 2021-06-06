using Microsoft.EntityFrameworkCore;
using PromoCodeManagement.Models;

namespace PromoCodeManagement.Entity
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=voucher;Username=postgres;Password=P@$$w0rd");
            }
        }
    }
}
