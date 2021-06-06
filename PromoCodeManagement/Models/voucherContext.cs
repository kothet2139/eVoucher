using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PromoCodeManagement.Models
{
    public partial class voucherContext : DbContext
    {
        public voucherContext()
        {
        }

        public voucherContext(DbContextOptions<voucherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=voucher;Username=postgres;Password=P@$$w0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasColumnType("timestamp with time zone");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.PaymentMethodid, "IX_Orders_payment_methodid");

                entity.HasIndex(e => e.Productid, "IX_Orders_productid");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CardExpiryDate).HasColumnName("card_expiry_date");

                entity.Property(e => e.CardNumber).HasColumnName("card_number");

                entity.Property(e => e.Cvv).HasColumnName("cvv");

                entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");

                entity.Property(e => e.GeneratedStatus).HasColumnName("generated_status");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.PaymentMethodid).HasColumnName("payment_methodid");

                entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");

                entity.Property(e => e.PhoneNo).HasColumnName("phone_no");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.TranId).HasColumnName("tran_id");

                entity.Property(e => e.TranStatus).HasColumnName("tran_status");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodid);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Productid);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.HasIndex(e => e.Productid, "IX_PaymentMethod_Productid");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PaymentMethods)
                    .HasForeignKey(d => d.Productid);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsOnlymeUsage).HasColumnName("is_onlyme_usage");

                entity.Property(e => e.MaxForMe).HasColumnName("max_for_me");

                entity.Property(e => e.MaxToGift).HasColumnName("max_to_gift");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            modelBuilder.Entity<PurchaseHistory>(entity =>
            {
                entity.HasIndex(e => e.Orderid, "IX_PurchaseHistories_orderid");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");

                entity.Property(e => e.IsUsed).HasColumnName("is_used");

                entity.Property(e => e.Orderid).HasColumnName("orderid");

                entity.Property(e => e.PromoCode).HasColumnName("promo_code");

                entity.Property(e => e.QrCode).HasColumnName("qr_code");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PurchaseHistories)
                    .HasForeignKey(d => d.Orderid);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
