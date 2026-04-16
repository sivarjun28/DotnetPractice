using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    Isbn = "1234567890123",
                    PublishedDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                    Pages = 328,
                    Publisher = "Secker & Warburg",
                    Price = 499,
                    AvailableCopies = 5,
                    TotalCopies = 5,
                    Description = "Dystopian novel",
                    CategoryId = 1,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}