using Microsoft.Extensions.Caching.Memory;

namespace Exercise01.MiddleWares
{
    public class CustomResponseCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CustomResponseCachingMiddleware> _logger;

        public CustomResponseCachingMiddleware(
            RequestDelegate next,
            IMemoryCache cache,
            ILogger<CustomResponseCachingMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                await _next(context);
                return;
            }

            var cacheKey = GenerateCacheKey(context.Request);

            if (_cache.TryGetValue(cacheKey, out CachedResponse? cachedResponse) && cachedResponse != null)
            {
                _logger.LogInformation($"Cache HIT for {cacheKey}");

                if (context.Request.Headers.IfNoneMatch == cachedResponse.ETag)
                {
                    context.Response.StatusCode = StatusCodes.Status304NotModified;
                    return;
                }

                context.Response.StatusCode = cachedResponse.StatusCode;
                context.Response.ContentType = cachedResponse.ContentType;
                context.Response.Headers.ETag = cachedResponse.ETag;
                await context.Response.WriteAsync(cachedResponse.Body);
                return;
            }

            _logger.LogInformation($"Cache MISS for {cacheKey}");

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            if (context.Response.StatusCode == 200)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(responseBody).ReadToEndAsync();

                var response = new CachedResponse
                {
                    StatusCode = context.Response.StatusCode,
                    ContentType = context.Response.ContentType ?? "application/json",
                    Body = body,
                    ETag = context.Response.Headers.ETag.ToString()
                };

                _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            return $"{request.Path}{request.QueryString}";
        }

        private class CachedResponse
        {
            public int StatusCode { get; set; }
            public string ContentType { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
            public string ETag { get; set; } = string.Empty;
        }
    }
}