using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasData(
                new Member
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    Phone = "9999999999",
                    Address = "Bangalore",

                    MembershipDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    MembershipExpiryDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),

                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}