using Exercise01.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Exercise01.Services
{
    public class ProductService : IProductService
    {

        private readonly IMemoryCache _cache;

        public ProductService(IMemoryCache cache)
        {
            _cache = cache;
        }
        private static readonly List<Product> _products = new();
        private static int _nextId = 1;
        public Task<Product> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = request.Name,
                Price = request.Price
            };

            _products.Add(product);
            _cache.Remove("/api/products");

            return Task.FromResult(product);
        }

        public Task<Product?> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return Task.FromResult<Product?>(null);

            _products.Remove(product);

            return Task.FromResult<Product?>(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string q, string? category)
        {
            var query = _products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Name.Contains(q));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await Task.FromResult(query.ToList());
        }


        public async Task<Product> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return null;

            product.Name = dto.Name;
            product.Price = dto.Price;

            return await Task.FromResult(product);
        }


    }
}