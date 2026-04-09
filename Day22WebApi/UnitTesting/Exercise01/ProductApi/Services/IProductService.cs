using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductApi.Models;
using ProductApi.Repositories;
using ProductApi.Models.ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(CreateProductDto createProductDto);
        Task<Product> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteAsync(int id);
        Task<List<Product>> SearchAsync(string searchTerm);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        private readonly ILogger _logger;
        private readonly IEventPublisher _eventPublisher;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, ICacheService cache, IEventPublisher eventPublisher)
        {
            _productRepository = productRepository;
            _logger = logger;
            _cache = cache;
            _eventPublisher = eventPublisher;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var cacheKey = "products:all";
            var cachedProducts = await _cache.GetAsync<List<Product>>(cacheKey);

            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = await _productRepository.GetAllAsync();
            await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(10));
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock
            };

            var createdProduct = await _productRepository.AddAsync(product);

            await _cache.RemoveAsync("products:all");

            await _eventPublisher.PublishAsync(new ProductCreatedEvent { ProductId = createdProduct.Id });

            return createdProduct;
        }

        public async Task<Product> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            product.Name = updateProductDto.Name;
            product.Price = (decimal)updateProductDto.Price;

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            await _productRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<Product>> SearchAsync(string searchTerm)
        {
            return await _productRepository.SearchAsync(searchTerm);
        }
    }
}