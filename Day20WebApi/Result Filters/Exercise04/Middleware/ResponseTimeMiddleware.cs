using System.Diagnostics;
namespace Exercise04.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();

            context.Response.Headers["X-Response-Time"] =
                $"{sw.ElapsedMilliseconds}ms";
        }
    }
}