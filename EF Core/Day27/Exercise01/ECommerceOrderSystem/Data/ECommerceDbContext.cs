using ECommerceOrderSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderSystem.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
                                : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Orders)
                        .WithOne(o => o.Customer)
                        .HasForeignKey(o => o.CustomerId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Addresses)
                        .WithOne(a => a.Customer)
                        .HasForeignKey(a => a.CustomerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                        .HasOne(o => o.Customer)
                        .WithMany(c => c.Orders)
                        .HasForeignKey(o => o.CustomerId)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
                        .HasOne(o => o.ShippingAddress)
                        .WithMany()
                        .HasForeignKey(o => o.ShippingAddressId)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
                        .HasOne(o => o.BillingAddress)
                        .WithMany()
                        .HasForeignKey(o => o.BillingAddressId)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
                        .HasMany(o => o.Items)
                        .WithOne(i => i.Order)
                        .HasForeignKey(i => i.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
                        .HasOne(o => o.Payment)
                        .WithOne(p => p.Order)
                        .HasForeignKey<Payment>(p => p.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>()
                        .HasOne(oi => oi.Order)
                        .WithMany(o => o.Items)
                        .HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<OrderItem>()
                        .HasOne(oi => oi.Product)
                        .WithMany(p => p.OrderItems)
                        .HasForeignKey(oi => oi.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>()
                        .HasIndex(oi => new { oi.OrderId, oi.ProductId })
                        .IsUnique();
            modelBuilder.Entity<Category>()
                        .HasOne(c => c.ParentCategory)
                        .WithMany(c => c.SubCategories)
                        .HasForeignKey(c => c.ParentCategoryId)
                        .OnDelete(DeleteBehavior.Restrict);
         modelBuilder.Entity<Address>()
                        .HasOne(a => a.Customer)
                        .WithMany(c => c.Addresses)
                        .HasForeignKey(a => a.CustomerId);
        modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Order)
                        .WithOne(o => o.Payment)
                        .HasForeignKey<Payment>(p => p.OrderId);





        }

    }
}