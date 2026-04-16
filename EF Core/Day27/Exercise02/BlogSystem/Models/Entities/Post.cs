using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public int ViewCount { get; set; }

        public Author Author{get; set;} = null!;
        public List<Comment> Comments{get; set;} = new();
        public List<Tag> Tags{get; set;} = new();
        public List<Category> Categories = new();

    }
}