using cms.eVoucher.Model;
using Microsoft.EntityFrameworkCore;

namespace cms.eVoucher.Entity
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
