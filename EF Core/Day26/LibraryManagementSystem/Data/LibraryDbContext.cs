using LibraryManagementSystem.Configuration;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
                    : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(b => b.Title);

                entity.Property(b => b.Isbn)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.HasIndex(b => b.Isbn)
                    .IsUnique();

                entity.Property(b => b.PublishedDate)
                    .IsRequired();

                entity.Property(b => b.Pages)
                    .IsRequired();

                entity.Property(b => b.Publisher)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(b => b.AvailableCopies)
                    .IsRequired();

                entity.Property(b => b.TotalCopies)
                    .IsRequired();

                entity.Property(b => b.Description)
                    .HasMaxLength(2000);

                entity.Property(b => b.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(b => b.LastModified)
                    .IsRowVersion();
                entity.HasOne(b => b.Category)
                        .WithMany(c => c.Books)
                        .HasForeignKey(b => b.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Authors");

                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(a => new { a.FirstName, a.LastName });

                entity.Property(a => a.Biography)
                    .HasMaxLength(5000);

                entity.Property(a => a.Country)
                    .HasMaxLength(50);

                entity.Property(a => a.CreatedAt)
                    .HasDefaultValueSql("NOW()");
            });
            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Members");

                entity.Property(m => m.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(m => m.Email)
                    .IsUnique();

                entity.Property(m => m.Phone)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(m => m.MembershipDate)
                    .HasDefaultValueSql("NOW()");

                entity.Property(m => m.IsActive)
                    .HasDefaultValue(true);

                entity.Property(m => m.Address)
                    .HasMaxLength(200);

                entity.HasIndex(m => m.IsActive);
            });
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loans");

                entity.Property(l => l.LoanDate)
                    .HasDefaultValueSql("NOW()");

                entity.Property(l => l.Status)
                    .HasDefaultValue(LoanStatus.Active);

                entity.Property(l => l.LateFee)
                    .HasColumnType("decimal(10,2)");

                entity.HasIndex(l => new { l.MemberId, l.Status });

                entity.HasIndex(l => l.DueDate);
            });

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());



        }
    }
}