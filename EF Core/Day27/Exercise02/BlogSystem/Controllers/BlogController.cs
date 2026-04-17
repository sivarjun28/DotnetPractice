using BlogSystem.Models.Requests;
using BlogSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                var post = await _blogService.CreatePostAsync(request);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{postId}/publish")]
        public async Task<IActionResult> PublishPost(int postId)
        {
            var post = await _blogService.PublishPostAsync(postId);
            return Ok(post);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var post = await _blogService.GetPostWithAllDataAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpGet("tag/{tagName}")]
        public async Task<IActionResult> GetPostsByTag(string tagName)
        {
            var posts = await _blogService.GetPostsByTagAsync(tagName);
            return Ok(posts);
        }

        [HttpGet("category/{categorySlug}")]
        public async Task<IActionResult> GetPostsByCategory(string categorySlug)
        {
            var posts = await _blogService.GetPostsByCategoryAsync(categorySlug);
            return Ok(posts);
        }

        [HttpPost("{postId}/tags")]
        public async Task<IActionResult> AddTags(int postId, [FromBody] List<string> tags)
        {
            await _blogService.AddTagsToPostAsync(postId, tags);
            return Ok("Tags added successfully");
        }

        [HttpPost("{postId}/categories")]
        public async Task<IActionResult> AddCategories(int postId, [FromBody] List<string> categories)
        {
            await _blogService.AddCategoriesToPostAsync(postId, categories);
            return Ok("Categories added successfully");
        }

        [HttpGet("tags/popular")]
        public async Task<IActionResult> GetPopularTags([FromQuery] int count = 10)
        {
            var tags = await _blogService.GetPopularTagsAsync(count);
            return Ok(tags);
        }
    }
}