using System.Net;
using Exercise06.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = ex.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => new { e.ErrorMessage, e.ErrorCode }));

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Validation Failed",
                status = 400,
                errors
            });
        }
        catch (BusinessRuleException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                title = "Business Rule Violation",
                code = ex.Code,
                message = ex.Message
            });
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                title = "Internal Server Error"
            });
        }
    }
}
