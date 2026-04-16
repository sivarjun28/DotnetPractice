using Exercise01.Models;
using Exercise01.Repository;

namespace Exercise01.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;
        private readonly ICacheService _cache;
        private readonly IEventPublisher _eventPublisher;

        public ProductService(IProductRepository repository,
                               ILogger<ProductService> logger,
                               ICacheService cache,
                               IEventPublisher eventPublisher)
        {
            _repository = repository;
            _logger = logger;
            _cache = cache;
            _eventPublisher = eventPublisher;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            const string cacheKey = "product_all";
            var cached = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Returned Products from cache");
                return cached;
            }
            var products = await _repository.GetAllAsync();
            await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Fetched Products from Repository");
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var cacheKey = $"product_{id}";
            var cached = await _cache.GetAsync<Product>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation($"Produc id {id} fetched from cache");
                return cached;
            }
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(5));
            }
            _logger.LogInformation($"Product {id} fetched from repository");

            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("product Name is Required");
            var created = await _repository.AddAsync(product);

            await _cache.RemoveByPrefixAsync("product_");
            await _cache.RemoveAsync("product_all");

            await _eventPublisher.PublishAsync(new ProductCreatedEvent
            {
                ProductId = created.Id,
                ProductName = created.Name,
                OccurredAt = DateTime.UtcNow
            });
            _logger.LogInformation($"Product created: {created.Id}");
            return created;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;
            existing.Name = product.Name;
            existing.Price = product.Price;

            var updated = await _repository.UpdateAsync(existing);
            await _cache.RemoveByPrefixAsync("product_");
            await _cache.RemoveAsync("product_all");

            if (updated != null)
            {
                await _eventPublisher.PublishAsync(new ProductUpdatedEvent
                {
                    ProductId = updated.Id,
                    OccurredAt = DateTime.UtcNow
                });
                _logger.LogInformation($"Product Updated: {updated.Id}");
            }
            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return false;

            await _cache.RemoveByPrefixAsync("product_");
            await _cache.RemoveAsync("products_all");

            await _eventPublisher.PublishAsync(new ProductDeletedEvent
            {
                ProductId = id,
                OccurredAt = DateTime.UtcNow

            });
            _logger.LogInformation($"Product Deleted{id}");
            return true;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchItem)
        {
            if (string.IsNullOrWhiteSpace(searchItem))
                return Enumerable.Empty<Product>();

            var results = await _repository.SearchAsync(searchItem);
            _logger.LogInformation($"Search Performed: {searchItem}");

            return results;
        }
    }
}