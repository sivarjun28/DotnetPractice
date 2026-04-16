using LibraryManagementSystem.Models.Entities;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}