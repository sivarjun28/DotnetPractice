namespace Exercise01.MiddleWare
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestValidationMiddleware> _logger;

        private const long MaxRequestSizeBytes = 1 * 1024 * 1024; // 1MB

        public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                if (!IsMultipart(context) &&
                    context.Request.ContentLength.HasValue &&
                    context.Request.ContentLength > MaxRequestSizeBytes)
                {
                    await Reject(context, StatusCodes.Status400BadRequest, "Request size exceeds 1MB limit");
                    return;
                }


                if (IsWriteMethod(context))
                {
                    if (!context.Request.Headers.ContainsKey("Content-Type"))
                    {
                        await Reject(context, 400, "Missing Content-Type header");
                        return;
                    }

                    if (!context.Request.Headers.ContainsKey("Accept"))
                    {
                        await Reject(context, 400, "Missing Accept header");
                        return;
                    }

                    var contentType = context.Request.ContentType;

                    if (!IsMultipart(context) &&
                        contentType != null &&
                        !contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        await Reject(context, 415, "Unsupported Content-Type. Only application/json is allowed");
                        return;
                    }
                }


                foreach (var param in context.Request.Query)
                {
                    if (LooksMalicious(param.Value))
                    {
                        _logger.LogWarning("Potentially malicious query parameter detected: {Param}", param.Key);

                        await Reject(context, 400, "Invalid query parameter");
                        return;
                    }
                }


                foreach (var key in context.Request.Query.Keys.ToList())
                {
                    var value = context.Request.Query[key].ToString().Trim();
                    context.Items[$"sanitized_{key}"] = value;
                }


                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { Error = ex.Message }); 
            }
        }

        private static bool IsWriteMethod(HttpContext context)
        {
            return context.Request.Method == HttpMethods.Post ||
                   context.Request.Method == HttpMethods.Put ||
                   context.Request.Method == HttpMethods.Patch;
        }

        private static bool IsMultipart(HttpContext context)
        {
            return context.Request.ContentType != null &&
                   context.Request.ContentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase);
        }

        private static bool LooksMalicious(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;


            var pattern = @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|UNION|ALTER|EXEC)\b)|(--|\|\||;|'|\*)";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        private async Task Reject(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}