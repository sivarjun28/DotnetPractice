using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "BookCount", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Fictional works", "Fiction" },
                    { 2, 0, "Factual works", "Non-Fiction" },
                    { 3, 0, "Scientific literature", "Science" },
                    { 4, 0, "Technology and computing", "Technology" },
                    { 5, 0, "Historical works", "History" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "MembershipDate", "MembershipExpiryDate", "Phone" },
                values: new object[] { 1, "Bangalore", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john@example.com", "John", true, "Doe", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "9999999999" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AvailableCopies", "CategoryId", "CreatedAt", "Description", "Isbn", "Pages", "Price", "PublishedDate", "Publisher", "Title", "TotalCopies" },
                values: new object[] { 1, 5, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dystopian novel", "1234567890123", 328, 499m, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Secker & Warburg", "1984", 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
