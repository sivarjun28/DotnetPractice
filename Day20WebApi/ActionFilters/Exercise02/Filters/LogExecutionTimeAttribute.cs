using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise02.Filters
{
    public class LogExecutionTimeAttribute : ActionFilterAttribute
    {

        private readonly ILogger<LogExecutionTimeAttribute> _logger;
        private readonly int _warningThresholdMs;

        public LogExecutionTimeAttribute(int warningThresholdMs = 1000)
        {
            _warningThresholdMs = warningThresholdMs;
        }

        public override async Task OnActionExecutionAsync(
         ActionExecutingContext context,
         ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var executedContext = await next();
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            var logger = (ILogger<LogExecutionTimeAttribute>)context.HttpContext.RequestServices
                .GetService(typeof(ILogger<LogExecutionTimeAttribute>));

            logger?.LogInformation("Action {Action} executed in {ElapsedMilliseconds} ms",
                context.ActionDescriptor.DisplayName, elapsedMs);

            if (elapsedMs > _warningThresholdMs)
            {
                logger?.LogWarning("Action {Action} exceeded threshold of {Threshold} ms",
                    context.ActionDescriptor.DisplayName, _warningThresholdMs);
            }

            context.HttpContext.Response.Headers["X-Execution-Time"] = elapsedMs.ToString();
        }
    }
}