namespace Exercise02.Models
{
    public class OrderDocumentUpload
    {
        public string DocumentType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<IFormFile> Files { get; set; } = new();
    }
}