namespace LibraryManagementSystem.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Biography { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}