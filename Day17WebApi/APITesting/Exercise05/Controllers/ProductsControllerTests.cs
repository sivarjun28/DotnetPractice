using System.Net;
using System.Net.Http.Json;
using Exercise05.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ProductsControllerTests
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient client;

    public ProductsControllerTests()
    {
        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {});
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
        var response = await client.GetAsync("/api/products/1");

        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_Returns404()
    {
        var response = await client.GetAsync("/api/products/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProduct_WithNonIntegerId_Returns404()
    {
        var response = await client.GetAsync("/api/products/abc");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("AB1234", true)]
    [InlineData("123456", false)]
    [InlineData("ABC123", false)]
    public async Task GetProductBySku_ValidatesSkuFormat(string sku, bool shouldSucceed)
    {
        var response = await client.GetAsync($"/api/products/sku/{sku}");

        if (shouldSucceed)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        else
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SearchProducts_WithQueryParams_ReturnsFilteredResults()
    {
        var response = await client.GetAsync("/api/products/search?name=Laptop");

        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();

        Assert.NotNull(products);
        Assert.All(products, p => Assert.Contains("Laptop", p.Name));
    }
}