using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class OrdersControllerTests
{
    private readonly HttpClient client;

    public OrdersControllerTests()
    {
        var factory = new WebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ReturnsCreated()
    {
        var order = new
        {
            ProductId = 1,
            Quantity = 2
        };

        var response = await client.PostAsJsonAsync("/api/orders", order);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_WithInvalidData_ReturnsBadRequest()
    {
        var order = new
        {
            ProductId = 0, // invalid
            Quantity = 0
        };

        var response = await client.PostAsJsonAsync("/api/orders", order);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateOrder_WithETag_UpdatesSuccessfully()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "/api/orders/1")
        {
            Content = JsonContent.Create(new { Quantity = 5 })
        };

        request.Headers.Add("If-Match", "\"etag-value\"");

        var response = await client.SendAsync(request);

        Assert.True(response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task BulkCreate_WithoutApiKey_ReturnsUnauthorized()
    {
        var orders = new[]
        {
            new { ProductId = 1, Quantity = 2 }
        };

        var response = await client.PostAsJsonAsync("/api/orders/bulk", orders);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}