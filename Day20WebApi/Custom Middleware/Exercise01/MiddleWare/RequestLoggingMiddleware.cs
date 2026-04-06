using System.Diagnostics;

namespace Execise01.MiddleWare
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["requestId"] = requestId;

            _logger.LogInformation("Request {RequestId}: {Method} {Path} {QueryString}",
                requestId, context.Request.Method, context.Request.Path, context.Request.QueryString);

            var stopWatch = Stopwatch.StartNew();

            // Register headers safely before response starts
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Request-Id"] = requestId;
                context.Response.Headers["X-Processing-Time"] = stopWatch.ElapsedMilliseconds.ToString();
                return Task.CompletedTask;
            });

            await _next(context);

            _logger.LogInformation("Request {RequestId} completed with status {StatusCode} in {Duration} ms. Content-Type: {ContentType}",
                requestId, context.Response.StatusCode, stopWatch.ElapsedMilliseconds, context.Response.ContentType);
        }

    }
}