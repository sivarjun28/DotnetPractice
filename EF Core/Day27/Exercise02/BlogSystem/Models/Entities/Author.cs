namespace BlogSystem.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Bio { get; set; }

        public List<Post> Posts{get; set;} = new();
    }
}