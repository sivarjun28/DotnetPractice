using Exercise04.Models;
using Exercise04.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Exercise04.Filters
{


    public class ResponseFormatterFilter : IResultFilter
    {
        private readonly HateoasService _hateoas;

        public ResponseFormatterFilter(HateoasService hateoas)
        {
            _hateoas = hateoas;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Skip non-object results (File, Empty, etc.)
            if (context.Result is not ObjectResult objectResult)
                return;

            // Avoid double wrapping
            if (objectResult.Value is ApiResponse<object>)
                return;

            var statusCode = objectResult.StatusCode ?? 200;
            var isSuccess = statusCode >= 200 && statusCode < 300;

            object? data = objectResult.Value;

            // 🔹 Handle HATEOAS for collections
            if (data is IEnumerable<object> list && list.Any())
            {
                data = _hateoas.AddLinks(context.HttpContext, list);
            }

            var response = new ApiResponse<object>
            {
                Success = isSuccess,
                Data = isSuccess ? data : null,
                Metadata = new ApiMetadata
                {
                    Timestamp = DateTime.UtcNow,
                    RequestId = context.HttpContext.TraceIdentifier,
                    Version = "2.0"
                },
                Errors = new List<string>()
            };

            // 🔹 Handle errors
            if (!isSuccess)
            {
                response.Errors.Add("Request failed");

                // Extract validation errors (ModelState)
                if (objectResult.Value is SerializableError errors)
                {
                    response.Errors = errors
                        .SelectMany(e => e.Value as string[] ?? new[] { e.Value?.ToString() ?? "" })
                        .ToList();
                }
            }

            // 🔹 Add pagination metadata (from PaginationFilter)
            if (context.HttpContext.Items.ContainsKey("Pagination"))
            {
                dynamic pg = context.HttpContext.Items["Pagination"];

                response.Metadata.TotalCount = pg.totalCount;
                response.Metadata.PageSize = pg.pageSize;
                response.Metadata.CurrentPage = pg.currentPage;
            }

            // 🔹 Replace final response
            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // 🔹 Optional: Logging / Metrics

            var response = context.HttpContext.Response;

            // Example: log status
            var statusCode = response.StatusCode;
            var path = context.HttpContext.Request.Path;

            Console.WriteLine($"[Response] {statusCode} -> {path}");
        }
    }
}