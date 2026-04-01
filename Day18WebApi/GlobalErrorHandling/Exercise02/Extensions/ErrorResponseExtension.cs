using Exercise02.Exceptions;
using Exercise02.Models;

namespace Exercise02.Extensions
{
    public static class ErrorResponseExtension
    {
        public static ErrorResponse ToErrorResponse(this Exception exception, HttpContext httpContext)
        {
            var traceId = httpContext.TraceIdentifier;
            return exception switch
            {
                NotFoundException nf => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "NotFound",
                    Message = nf.Message,
                    TraceId = traceId
                },
                ValidationException ve => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Message = ve.Message,
                    TraceId = traceId
                },
                DuplicateException de => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Title = "Duplication Resource",
                    Message = de.Message,
                    TraceId = traceId
                },
                BusinessRuleException bre => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Business Rule Violation",
                    Message = bre.Message,
                    TraceId = traceId,
                    HelpLink = $"https://docs.yourapi.com/errors/{bre.RuleCode}"
                },
                InsufficientStockException ise => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Insufficient resorce",
                    Message = ise.Message,
                    TraceId = traceId
                },
                _ => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Message = "An unexpected error occurred.",
                    TraceId = traceId
                }
            };
        }
    }
}