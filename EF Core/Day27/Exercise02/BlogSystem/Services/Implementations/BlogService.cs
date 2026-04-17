using BlogSystem.Data;
using BlogSystem.Models.Entities;
using BlogSystem.Models.Requests;
using BlogSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly BlogDbContext _context;
        public BlogService(BlogDbContext context)
        {
            _context = context;
        }
        public async Task AddCategoriesToPostAsync(int postId, List<string> categoryNames)
        {
            var post = await _context.Posts
                    .Include(post => post.Categories)
                    .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                throw new Exception("Post not found");

            foreach (var categoryName in categoryNames.Distinct())
            {
                var slug = categoryName.ToLower().Replace(" ", "-");
                if (post.Categories.Any(c => c.Slug == slug))
                {
                    continue;
                }
                var category = await _context.Categories
                                    .FirstOrDefaultAsync(c => c.Slug == slug);
                if (category == null)
                {
                    category = new Category
                    {
                        Name = categoryName,
                        Slug = slug
                    };
                    _context.Categories.Add(category);

                }
                post.Categories.Add(category);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddTagsToPostAsync(int postId, List<string> tagNames)
        {
            var post = await _context.Posts
                            .Include(p => p.Tags)
                            .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                throw new Exception("Post NOt Found");

            foreach (var tagName in tagNames.Distinct())
            {
                if (post.Tags.Any(t => t.Name == tagName))
                    continue;

                var tag = await _context.Tags
              .FirstOrDefaultAsync(t => t.Name == tagName);

                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName,
                        UsageCount = 0
                    };
                    _context.Tags.Add(tag);
                }
                tag.UsageCount++;
                post.Tags.Add(tag);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Post> CreatePostAsync(CreatePostRequest request)
        {
            var post = new Post
            {
                AuthorId = request.AuthorId,
                Title = request.Title,
                Content = request.Content,
                PublishedDate = DateTime.UtcNow,
                IsPublished = true,
                Tags = new List<Tag>(),
                Categories = new List<Category>()
            };

            foreach (var tagName in request.Tags.Distinct())
            {
                var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == tagName);

                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName,
                        UsageCount = 0
                    };
                    _context.Tags.Add(tag);
                }
                tag.UsageCount++;
                post.Tags.Add(tag);

            }

            foreach (var categoryName in request.Categories.Distinct())
            {
                var slug = categoryName.ToLower().Replace(" ", "-");

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Slug == slug);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = categoryName,
                        Slug = slug
                    };
                    _context.Categories.Add(category);
                }

                post.Categories.Add(category);
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<List<Tag>> GetPopularTagsAsync(int count)
        {
            return await _context.Tags
                        .OrderByDescending(t => t.UsageCount)
                        .Take(count)
                        .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(string categorySlug)
        {
            return await _context.Posts
        .Where(p => p.IsPublished &&
                    p.Categories.Any(c => c.Slug == categorySlug))
        .Include(p => p.Author)
        .Include(p => p.Tags)
        .Include(p => p.Categories)
        .Include(p => p.Comments)
        .OrderByDescending(p => p.PublishedDate)
        .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByTagAsync(string tagName)
        {
            return await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Categories)
                .Include(p => p.Author)
                .Where(p => p.IsPublished &&
                             p.Tags.Any(t => t.Name == tagName))
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();
        }
        public async Task<Post?> GetPostWithAllDataAsync(int postId)
        {
            var post = await _context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Tags)
                        .Include(p => p.Categories)
                        .Include(p => p.Comments)
                            .ThenInclude(c => c.Replies)
                        .FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                post.ViewCount++;
                await _context.SaveChangesAsync();
            }
            return post;

        }

        public async Task<Post> PublishPostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            post.IsPublished = true;
            post.PublishedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return post;
        }
    }
}