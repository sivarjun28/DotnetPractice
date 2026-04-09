using Exercise01.Models;

namespace Exercise01.Services
{
    public class ProductService : IProductService
    {

        private static readonly List<Product> _products = new();
        private static int _nextId = 1;
        public Task<Product> CreateAsync(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return Task.FromResult(product);
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

        public Task UpdateAsync(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);

            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.Category = product.Category;
                existing.Stock = product.Stock;
                existing.Tags = product.Tags;
                existing.UpdatedAt = product.UpdatedAt;
            }

            return Task.CompletedTask;
        }
    }
}