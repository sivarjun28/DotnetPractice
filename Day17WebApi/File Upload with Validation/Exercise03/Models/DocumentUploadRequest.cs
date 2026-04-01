namespace Exercise03.Models
{
    public class DocumentUploadRequest
    {
        public required IFormFile File { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Category { get; set; }
        public List<string> Tags { get; set; } = new();
        public bool IsPublic { get; set; }
    }
}