using FileUploadApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
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

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<ErrorResponse>
            {
                Success = false,
                Message = "An unexpected error occurred.",
                Data = new ErrorResponse
                {
                    Error = exception.Message,
                    Details = exception.StackTrace
                }
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}