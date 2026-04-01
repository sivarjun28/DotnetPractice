namespace Exercise03.Models
{
    public class DocumentMetadata
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public bool IsPublic { get; set; }
    }
}