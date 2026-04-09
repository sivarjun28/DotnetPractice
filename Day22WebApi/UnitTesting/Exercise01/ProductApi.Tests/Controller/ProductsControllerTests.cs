using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Controllers;
using ProductApi.Models;
using ProductApi.Models.ProductApi.Models;
using ProductApi.Services;
using Xunit;

namespace ProductApi.Tests.Controller
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockProductService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfProducts()
        {
            var expectedProducts = new List<Product>
            {
                new Product{Id = 1, Name ="Product 1", Price = 1099.8m},
                new Product{Id = 2, Name ="Product 2", Price = 1099.8m}
            };
            _mockProductService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedProducts);
            var result = await _controller.GetAll();
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnOkResult_WithProduct()
        {
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Test Product", Price = 99.9m };
            _mockProductService
            .Setup(s => s.GetByIdAsync(productId))
            .ReturnsAsync(expectedProduct);

            var result = await _controller.GetById(productId);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedProduct);
        }

                [Fact]
        public async Task Create_WithValidProduct_ReturnsCreatedAction()
        {
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 456.9m,
                Stock = 100
            };
            var createdProduct = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };
            _mockProductService
                    .Setup(s => s.CreateAsync(createDto))
                    .ReturnsAsync(createdProduct);

            var result = await _controller.Create(createDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result); 
            Assert.Equal("GetById", createdAtActionResult.ActionName); 
            Assert.NotNull(createdAtActionResult.Value);
        }

        [Fact]
        public async Task Update_WithValidData_ReturnsOkResult()
        {
            var productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = 78.99m
            };

            var updatedProduct = new Product
            {
                Id = productId,
                Name = updateDto.Name,
                Price = (decimal)updateDto.Price
            };
            _mockProductService
           .Setup(s => s.UpdateAsync(productId, updateDto))
           .ReturnsAsync(updatedProduct);

            var result = await _controller.Update(productId, updateDto);

            result.Should().BeOfType<OkObjectResult>()
                     .Which.Value.Should().BeEquivalentTo(updatedProduct);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            var productId = 1;

            _mockProductService
                .Setup(s => s.DeleteAsync(productId))
                .ReturnsAsync(true);

            var result = await _controller.Delete(productId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnNotFound()
        {
            var productId = 999;
            _mockProductService
                .Setup(s => s.DeleteAsync(productId))
                .ReturnsAsync(false);
            var result = await _controller.Delete(productId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData("laptop")]
        [InlineData("phone")]
        [InlineData("tablet")]
        public async Task Search_WithSearchTerm_ReturnsMatchingProducts(string searchTerm)
        {
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = $"{searchTerm} Pro", Price = 999m }
            };

            _mockProductService
                    .Setup(s => s.SearchAsync(searchTerm))
                    .ReturnsAsync(expectedProducts);

            var result = await _controller.Search(searchTerm);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetAll_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            _mockProductService
                .Setup(s => s.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.GetAll();

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetById_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var productId = 1;
            _mockProductService
                .Setup(s => s.GetByIdAsync(productId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.GetById(productId);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Create_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 456.9m,
                Stock = 100
            };

            _mockProductService
                .Setup(s => s.CreateAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Create(createDto);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Update_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var productId = 1;
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = 78.99m
            };

            _mockProductService
                .Setup(s => s.UpdateAsync(productId, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Update(productId, updateDto);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Delete_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var productId = 1;
            _mockProductService
                .Setup(s => s.DeleteAsync(productId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Delete(productId);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Search_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var searchTerm = "laptop";
            _mockProductService
                .Setup(s => s.SearchAsync(searchTerm))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Search(searchTerm);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }
    }
}