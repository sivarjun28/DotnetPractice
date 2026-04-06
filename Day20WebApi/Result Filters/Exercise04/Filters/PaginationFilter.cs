namespace Exercise04.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class PaginationFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is IEnumerable<object> items)
            {
                var query = context.HttpContext.Request.Query;

                int page = int.TryParse(query["page"], out var p) ? p : 1;
                int pageSize = int.TryParse(query["pageSize"], out var ps) ? ps : 10;

                var list = items.ToList();
                int totalCount = list.Count;

                var pagedData = list
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

               
                context.HttpContext.Response.Headers["X-Total-Count"] = totalCount.ToString();

                var baseUrl = context.HttpContext.Request.Path;

                if ((page * pageSize) < totalCount)
                {
                    context.HttpContext.Response.Headers["Link"] =
                        $"<{baseUrl}?page={page + 1}&pageSize={pageSize}>; rel=\"next\"";
                }

                objectResult.Value = pagedData;

                
                context.HttpContext.Items["Pagination"] = new
                {
                    totalCount,
                    pageSize,
                    currentPage = page
                };
            }

            await next();
        }
    }
}