using Microsoft.AspNetCore.Mvc.Filters;
using System.IO.Compression;
using System.Text;
namespace Exercise04.Filters
{
    

public class CompressionFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var response = context.HttpContext.Response;

        var acceptEncoding = request.Headers["Accept-Encoding"].ToString();

        if (!acceptEncoding.Contains("gzip"))
        {
            await next();
            return;
        }

        var originalBody = response.Body;

        using var compressedStream = new MemoryStream();

        response.Body = compressedStream;

        await next();

        compressedStream.Seek(0, SeekOrigin.Begin);

        using var gzip = new GZipStream(originalBody, CompressionMode.Compress, true);
        await compressedStream.CopyToAsync(gzip);

        response.Headers["Content-Encoding"] = "gzip";
        response.Body = originalBody;
    }
}
}