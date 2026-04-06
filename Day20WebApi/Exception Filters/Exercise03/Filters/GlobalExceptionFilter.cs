using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Exercise03.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError(ex, "Unhandled Exception occurred. TraceId: {TraceId}", context.HttpContext.TraceIdentifier);

            var statusCode = ex switch
            {
                ArgumentException => 400,
                UnauthorizedAccessException => 401,
                _ => 500
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = ex.GetType().Name,
                Detail = _env.IsDevelopment() ? ex.ToString() : "An unexpected error occurred.",
                Instance = context.HttpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };

            context.ExceptionHandled = true;
        }
    }
}