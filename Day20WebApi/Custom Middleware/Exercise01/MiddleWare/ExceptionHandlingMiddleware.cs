using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Execise01.MiddleWare
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public readonly IHostEnvironment _env;
        private const string CorrelationIdItemKey = "CorrelationId";
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var correlationId = GetCorrelationId(context);


                var (statusCode, message) = GetErrorDetails(ex);

              
                using (_logger.BeginScope(new System.Collections.Generic.Dictionary<string, object>
                {
                    ["CorrelationId"] = correlationId
                }))
                {
                    _logger.LogError(ex,
                        "Unhandled exception occurred. CorrelationId: {CorrelationId}",
                        correlationId);
                }

                
                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                
                if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                {
                    context.Response.Headers.Add(CorrelationIdHeader, correlationId);
                }

                var response = new
                {
                    correlationId,
                    statusCode,
                    message = _env.IsDevelopment() ? message : "An unexpected error occurred.",
                    details = _env.IsDevelopment() ? ex.ToString() : null
                };

               
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = _env.IsDevelopment()
                });

               
                await context.Response.WriteAsync(json);
            }
        }

        private string GetCorrelationId(HttpContext context)
        {
            if (context.Items.TryGetValue(CorrelationIdItemKey, out var value) &&
                value is string cid && !string.IsNullOrWhiteSpace(cid))
            {
                return cid;
            }

            
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var headerValue) &&
                !string.IsNullOrWhiteSpace(headerValue))
            {
                return headerValue!;
            }

            return Guid.NewGuid().ToString();
        }


        private (int statusCode, string message) GetErrorDetails(Exception ex)
        {
            return ex switch
            {
                ValidationException => (400, ex.Message),
                UnauthorizedAccessException => (401, "Unauthorized"),
                KeyNotFoundException => (404, "Resource not found"),
                InvalidOperationException => (409, ex.Message),
                _ => (500, "An error occurred processing your request")
            };
        }
    }

}