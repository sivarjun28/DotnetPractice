using Exercise02.Models;

namespace Exercise02.Services
{
    public class ProductService : IProductService
    {
        private readonly IDistributedCacheService _cache;
        private readonly ILogger<ProductService> _logger;

        private static readonly List<Product> _products = new();
        private static int _idCounter = 1;

        private const string ProductCacheKeyPrefix = "product:";
        private const string AllProductsCacheKey = "products:all";

        public ProductService(
            IDistributedCacheService cache,
            ILogger<ProductService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var cachedProducts = await _cache.GetAsync<List<Product>>(AllProductsCacheKey);

            if (cachedProducts != null)
            {
                _logger.LogInformation("Returning products from cache");
                return cachedProducts;
            }

            _logger.LogInformation("Cache miss - fetching from memory store");

            var products = _products.ToList();

            await _cache.SetAsync(AllProductsCacheKey, products, TimeSpan.FromMinutes(5));

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var cacheKey = $"{ProductCacheKeyPrefix}{id}";

            var cachedProduct = await _cache.GetAsync<Product>(cacheKey);

            if (cachedProduct != null)
            {
                _logger.LogInformation($"Product {id} returned from cache");
                return cachedProduct;
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));
            }

            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.Id = _idCounter++;
            _products.Add(product);

            _logger.LogInformation($"Product created with Id {product.Id}");

            await _cache.RemoveAsync(AllProductsCacheKey);

            await _cache.SetAsync(
                $"{ProductCacheKeyPrefix}{product.Id}",
                product,
                TimeSpan.FromMinutes(10));

            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);

            if (existing == null)
                return null;

            existing.Name = product.Name;
            existing.Price = product.Price;

            _logger.LogInformation($"Product {id} updated");

            await _cache.RemoveAsync($"{ProductCacheKeyPrefix}{id}");
            await _cache.RemoveAsync(AllProductsCacheKey);

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return false;

            _products.Remove(product);

            _logger.LogInformation($"Product {id} deleted");

            await _cache.RemoveAsync($"{ProductCacheKeyPrefix}{id}");
            await _cache.RemoveAsync(AllProductsCacheKey);

            return true;
        }
    }
}