using BlogSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
                            : base(options)
        {
            
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Post>()
        .HasOne(p => p.Author)
        .WithMany(a => a.Posts)
        .HasForeignKey(p => p.AuthorId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Post>()
        .HasMany(p => p.Tags)
        .WithMany(t => t.Posts)
        .UsingEntity<Dictionary<string, object>>(
            "PostTags",
            j => j
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey("TagId")
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey("PostId")
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.HasKey("PostId", "TagId");
                j.ToTable("PostTags");
            });

    modelBuilder.Entity<Post>()
        .HasMany(p => p.Categories)
        .WithMany(c => c.Posts)
        .UsingEntity<Dictionary<string, object>>(
            "PostCategories",
            j => j
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey("PostId")
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.HasKey("PostId", "CategoryId");
                j.ToTable("PostCategories");
            });

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.Post)
        .WithMany(p => p.Comments)
        .HasForeignKey(c => c.PostId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.ParentComment)
        .WithMany(c => c.Replies)
        .HasForeignKey(c => c.ParentCommentId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Tag>()
        .HasIndex(t => t.Name)
        .IsUnique();

    modelBuilder.Entity<Category>()
        .HasIndex(c => c.Slug)
        .IsUnique();

    modelBuilder.Entity<Post>()
        .HasIndex(p => p.PublishedDate);

    modelBuilder.Entity<Post>()
        .HasIndex(p => p.IsPublished);

    modelBuilder.Entity<Post>()
        .HasIndex(p => new { p.IsPublished, p.PublishedDate });
}
    }
}