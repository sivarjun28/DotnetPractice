// In Exercise05.Tests/Controllers/OrdersControllerTests.cs
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Exercise05.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Exercise05.Tests.Controllers
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Test 1: Create Order with Valid Data
        [Fact]
        public async Task CreateOrder_WithValidData_ReturnsCreated()
        {
            var order = new { ProductId = 1, Quantity = 2 };

            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/orders", content);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        // Test 2: Create Order with Invalid Data (Validation Error)
        [Fact]
        public async Task CreateOrder_WithInvalidData_ReturnsBadRequest()
        {
            var order = new { ProductId = 0, Quantity = -1 }; // Invalid data (ProductId should be > 0, Quantity should be > 0)

            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/orders", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // Test 3: Get Order with Invalid ID (Not Found)
        [Fact]
        public async Task GetOrder_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/orders/999");  // Non-existent order ID

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // Test 4: Update Status with Invalid Transition (Unprocessable Entity)
        [Fact]
        public async Task UpdateStatus_WithInvalidTransition_ReturnsUnprocessableEntity()
        {
            var order = new { Status = "InvalidStatus" };  // Invalid status transition

            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/orders/1/status", content);

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        // Test 5: Cancel Order when Already Shipped (Conflict)
        [Fact]
        public async Task CancelOrder_WhenAlreadyShipped_ReturnsConflict()
        {
            var response = await _client.DeleteAsync("/api/orders/1");

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);  // Conflict because the order is already shipped
        }
    }
}