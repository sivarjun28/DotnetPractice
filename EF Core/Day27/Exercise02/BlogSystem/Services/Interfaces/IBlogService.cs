using BlogSystem.Models.Entities;
using BlogSystem.Models.Requests;

namespace BlogSystem.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Post> CreatePostAsync(CreatePostRequest request);
        Task<Post> PublishPostAsync(int postId);
        Task<Post?> GetPostWithAllDataAsync(int postId);
        Task<List<Post>> GetPostsByTagAsync(string tagName);
        Task<List<Post>> GetPostsByCategoryAsync(string categorySlug);
        Task AddTagsToPostAsync(int postId, List<string> tagNames);
        Task AddCategoriesToPostAsync(int postId, List<string> categoryNames);
        Task<List<Tag>> GetPopularTagsAsync(int count);
    }
}