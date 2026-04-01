using Exercise02.Extensions;
using Microsoft.AspNetCore.Diagnostics;

namespace Exercise02.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public readonly ILogger<GlobalExceptionHandler> logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            var errorResponse = exception.ToErrorResponse(httpContext);

            if (errorResponse.StatusCode >= 500)
                logger.LogError(exception, errorResponse.Message);
            else if (errorResponse.StatusCode >= 400)
                logger.LogWarning(exception, errorResponse.Message);
            else
                logger.LogInformation(exception, errorResponse.Message);
            response.StatusCode = errorResponse.StatusCode;
            await response.WriteAsJsonAsync(errorResponse, cancellationToken: cancellationToken);
            return true;
        }
    }
}