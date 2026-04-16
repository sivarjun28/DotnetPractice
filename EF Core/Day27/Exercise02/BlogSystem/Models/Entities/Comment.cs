namespace BlogSystem.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; } 
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }

        public Post Post { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public List<Comment> Replies { get; set; } = new();
    }
}