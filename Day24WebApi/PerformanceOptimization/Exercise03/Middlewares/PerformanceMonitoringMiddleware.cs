using System.Diagnostics;

namespace Exercise03.Middlwares
{
    using System.Diagnostics;


    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;

        public PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();

                var elapsedMs = sw.ElapsedMilliseconds;

                // ⚠️ IMPORTANT FIX
                if (!context.Response.HasStarted)
                {
                    context.Response.Headers["X-Response-Time-ms"] = elapsedMs.ToString();
                }

                if (elapsedMs > 1000)
                {
                    _logger.LogWarning(
                        "Slow request: {Method} {Path} took {ElapsedMs}ms",
                        context.Request.Method,
                        context.Request.Path,
                        elapsedMs);
                }
            }
        }
    }
}