using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository repository,
            IDistributedCache cache,
            ILogger<ProductService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var cacheKey = "products_all";
            var cachedProducts = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedProducts))
            {
                return JsonConvert.DeserializeObject<List<ProductResponse>>(cachedProducts);
            }

            var products = await _repository.GetAllAsync();
            var productResponses = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Sku = p.Sku,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Unknown",
                Stock = p.Stock,
                IsActive = p.IsActive,
                Tags = p.Tags,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(productResponses), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return productResponses;
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var cacheKey = $"product_{id}";
            var cachedProduct = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedProduct))
            {
                return JsonConvert.DeserializeObject<ProductResponse>(cachedProduct);
            }

            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            var productResponse = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Sku = product.Sku,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Unknown",
                Stock = product.Stock,
                IsActive = product.IsActive,
                Tags = product.Tags,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(productResponse), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return productResponse;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Sku = request.Sku,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Stock = request.Stock,
                IsActive = request.IsActive,
                Tags = request.Tags
            };

            var createdProduct = await _repository.AddAsync(product);

            var response = new ProductResponse
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Sku = createdProduct.Sku,
                Price = createdProduct.Price,
                CategoryId = createdProduct.CategoryId,
                CategoryName = createdProduct.Category?.Name ?? "Unknown",
                Stock = createdProduct.Stock,
                IsActive = createdProduct.IsActive,
                Tags = createdProduct.Tags,
                CreatedAt = createdProduct.CreatedAt,
                UpdatedAt = createdProduct.UpdatedAt
            };

            await _cache.RemoveAsync("products_all");

            return response;
        }

        public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = request.Name ?? product.Name;
            product.Description = request.Description ?? product.Description;
            product.Sku = request.Sku ?? product.Sku;
            product.Price = request.Price ?? product.Price;
            product.CategoryId = request.CategoryId ?? product.CategoryId;
            product.Stock = request.Stock ?? product.Stock;
            product.IsActive = request.IsActive ?? product.IsActive;
            product.Tags = request.Tags ?? product.Tags;

            var updatedProduct = await _repository.UpdateAsync(product);

            var response = new ProductResponse
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Sku = updatedProduct.Sku,
                Price = updatedProduct.Price,
                CategoryId = updatedProduct.CategoryId,
                CategoryName = updatedProduct.Category?.Name ?? "Unknown",
                Stock = updatedProduct.Stock,
                IsActive = updatedProduct.IsActive,
                Tags = updatedProduct.Tags,
                CreatedAt = updatedProduct.CreatedAt,
                UpdatedAt = updatedProduct.UpdatedAt
            };

            await _cache.RemoveAsync("products_all");
            await _cache.RemoveAsync($"product_{id}");

            return response;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (success)
            {
                await _cache.RemoveAsync("products_all");
                await _cache.RemoveAsync($"product_{id}");
            }
            return success;
        }

        public async Task<PagedResult<ProductResponse>> SearchAsync(ProductSearchCriteria criteria)
        {
            var result = await _repository.SearchAsync(criteria);

            var response = new PagedResult<ProductResponse>
            {
                TotalCount = result.TotalCount,
                Items = result.Items.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Sku = p.Sku,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name ?? "Unknown",
                    Stock = p.Stock,
                    IsActive = p.IsActive,
                    Tags = p.Tags,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
            };

            return response;
        }
    }
}