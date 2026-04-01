using Exercise04;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace Exercise04.Tests
{
    public class ReportsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ReportsControllerTests(WebApplicationFactory<Program> factory)
        {
            // Override content root to point to main project folder
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "../Exercise04"));
            }).CreateClient();
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkWithJsonContent()
        {
            var response = await _client.GetAsync("/api/reports/products");
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                         response.Content.Headers.ContentType?.ToString());
        }
    }
}