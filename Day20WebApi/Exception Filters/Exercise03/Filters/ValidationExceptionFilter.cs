using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Exercise03.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ValidationExceptionFilter> _logger;

        public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not ValidationException ex)
                return;

            var problemDetails = new ValidationProblemDetails(ex.Errors)
            {
                Status = 400,
                Title = "One or more validation errors occurred."
            };

            _logger.LogWarning("Validation error: {@errors}", ex.Errors);

            context.Result = new BadRequestObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }

    public class ValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}