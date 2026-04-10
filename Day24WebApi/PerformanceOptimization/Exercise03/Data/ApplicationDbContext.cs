using Exercise03.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise03.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Image> Images => Set<Image>();
         public DbSet<Order> Orders { get; set; }
    }
}