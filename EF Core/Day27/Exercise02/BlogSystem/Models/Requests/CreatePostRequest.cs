namespace BlogSystem.Models.Requests
{
    public class CreatePostRequest
    {
        public int AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}