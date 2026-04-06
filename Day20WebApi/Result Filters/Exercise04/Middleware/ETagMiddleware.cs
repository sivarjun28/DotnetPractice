using System.Security.Cryptography;
using System.Text;
namespace Exercise04.Middleware
{
public class ETagMiddleware
{
    private readonly RequestDelegate _next;

    public ETagMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBody = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(memoryStream).ReadToEndAsync();

        var hash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(body))
        );

        var etag = $"\"{hash}\"";

        context.Response.Headers["ETag"] = etag;

        if (context.Request.Headers["If-None-Match"] == etag)
        {
            context.Response.StatusCode = 304;
            context.Response.Body = originalBody;
            return;
        }

        context.Response.Body = originalBody;
        await context.Response.WriteAsync(body);
    }
}
}