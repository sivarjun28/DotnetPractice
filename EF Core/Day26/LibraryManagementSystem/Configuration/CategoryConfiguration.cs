using LibraryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Fiction", Description = "Fictional works", BookCount = 0 },
                new Category { Id = 2, Name = "Non-Fiction", Description = "Factual works", BookCount = 0 },
                new Category { Id = 3, Name = "Science", Description = "Scientific literature", BookCount = 0 },
                new Category { Id = 4, Name = "Technology", Description = "Technology and computing", BookCount = 0 },
                new Category { Id = 5, Name = "History", Description = "Historical works", BookCount = 0 }

            );
        }
    }
}