using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Models;
using ProductApi.Models.ProductApi.Models;
using ProductApi.Repositories;
using ProductApi.Services;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<ICacheService> _mockCache;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mockCache = new Mock<ICacheService>();
            _mockEventPublisher = new Mock<IEventPublisher>();

            _service = new ProductService(
                _mockRepository.Object,
                _mockLogger.Object,
                _mockCache.Object,
                _mockEventPublisher.Object);
        }

        [Fact]
        public async Task GetAllAsync_ChecksCacheFirst()
        {
            var cachedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Cached Product" }
            };

            _mockCache
                .Setup(c => c.GetAsync<List<Product>>("products:all"))
                .ReturnsAsync(cachedProducts);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(cachedProducts);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_WhenCacheMiss_FetchesFromRepository()
        {
            _mockCache
                .Setup(c => c.GetAsync<List<Product>>("products:all"))
                .ReturnsAsync((List<Product>?)null);

            var repositoryProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Repository Product" }
            };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(repositoryProducts);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(repositoryProducts);
            _mockCache.Verify(c => c.SetAsync("products:all", repositoryProducts, It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_PublishesEvent()
        {
            var createDto = new CreateProductDto { Name = "New Product", Price = 99.99m };
            var createdProduct = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };
            _mockRepository
                .Setup(r => r.AddAsync(createdProduct))
                .ReturnsAsync(createdProduct);

            var result = await _service.CreateAsync(createDto);

            _mockEventPublisher.Verify(
                e => e.PublishAsync(It.Is<ProductCreatedEvent>(evt => evt.ProductId == createdProduct.Id)),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InvalidatesCacheAsync()
        {
           
            var createDto = new CreateProductDto { Name = "New Product", Price = 99.99m };
            var createdProduct = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };

            _mockRepository
                .Setup(r => r.AddAsync(createdProduct))
                .ReturnsAsync(createdProduct);

            var result = await _service.CreateAsync(createDto);

            _mockCache.Verify(c => c.RemoveAsync("products:all"), Times.Once);
        }
    }
}