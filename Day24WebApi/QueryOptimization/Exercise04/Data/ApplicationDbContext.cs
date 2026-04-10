using Exercise04.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise04.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                :base(options)
        {
            
        }

        public DbSet<Product> Products {get; set;}
    }
}