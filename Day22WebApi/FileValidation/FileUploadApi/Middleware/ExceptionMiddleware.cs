using FileUploadApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileUploadApi.Middleware
{

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;

            switch (exception)
            {
                case ArgumentException:
                    status = HttpStatusCode.BadRequest;
                    break;

                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = exception.Message,
                Errors = new List<string> { exception.ToString() }
            };

            var result = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(result);
        }
    }
}