using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise02.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private static readonly ConcurrentDictionary<string, CachedResponse> _cache = new();
        private readonly TimeSpan _duration;

        public CacheAttribute(int seconds = 30)
        {
            _duration = TimeSpan.FromSeconds(seconds);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = GenerateCacheKey(context);

            if (_cache.TryGetValue(cacheKey, out var cached) && cached.ExpiresAt > DateTime.UtcNow)
            {
                context.Result = new ObjectResult(cached.Value) { StatusCode = 200 };
                context.HttpContext.Response.Headers["ETag"] = cached.ETag;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult result)
            {
                var eTag = Guid.NewGuid().ToString();
                _cache[cacheKey] = new CachedResponse
                {
                    Value = result.Value,
                    ExpiresAt = DateTime.UtcNow.Add(_duration),
                    ETag = eTag
                };

                context.HttpContext.Response.Headers["Cache-Control"] = $"public,max-age={_duration.TotalSeconds}";
                context.HttpContext.Response.Headers["ETag"] = eTag;
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var path = request.Path.ToString();
            var query = request.QueryString.ToString();
            var user = context.HttpContext.User.Identity?.Name ?? "anonymous";
            return $"{path}{query}:{user}";
        }

        private class CachedResponse
        {
            public object? Value { get; set; }
            public DateTime ExpiresAt { get; set; }
            public string ETag { get; set; } = string.Empty;
        }
    }

}