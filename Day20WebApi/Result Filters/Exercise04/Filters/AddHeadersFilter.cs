using Microsoft.AspNetCore.Mvc.Filters;
namespace Exercise04.Filters
{
    

public class AddHeadersFilter : IResultFilter
{
    private readonly Dictionary<string, string> _headers;

    public AddHeadersFilter(Dictionary<string, string> headers)
    {
        _headers = headers;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        foreach (var header in _headers)
        {
            context.HttpContext.Response.Headers[header.Key] = header.Value;
        }

        // Add Request ID automatically
        context.HttpContext.Response.Headers["X-Request-ID"] =
            context.HttpContext.TraceIdentifier;
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
}