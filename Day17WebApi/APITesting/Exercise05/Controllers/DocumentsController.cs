using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class DocumentsControllerTests
{
    private readonly HttpClient client;

    public DocumentsControllerTests()
    {
        var factory = new WebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    [Fact]
    public async Task Upload_WithValidFile_ReturnsSuccess()
    {
        var content = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("dummy file content"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        content.Add(fileContent, "file", "test.pdf");

        var response = await client.PostAsync("/api/documents/upload", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Upload_WithInvalidFileType_ReturnsBadRequest()
    {
        var content = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("exe file"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

        content.Add(fileContent, "file", "malware.exe");

        var response = await client.PostAsync("/api/documents/upload", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Upload_WithLargeFile_ReturnsBadRequest()
    {
        var largeContent = new byte[10 * 1024 * 1024]; // 10 MB

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(largeContent);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        content.Add(fileContent, "file", "large.pdf");

        var response = await client.PostAsync("/api/documents/upload", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}