using ProductApi.Models;
using ProductApi.Models.ProductApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000.99m, Stock = 10, Category = "Electronics" },
            new Product { Id = 2, Name = "Phone", Price = 500.99m, Stock = 20, Category = "Electronics" }
        };

        public Task<List<Product>> GetAllAsync() => Task.FromResult(_products);

        public Task<Product> GetByIdAsync(int id) =>
            Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public Task<Product> AddAsync(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product> UpdateAsync(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index >= 0)
            {
                _products[index] = product;
                return Task.FromResult(product);
            }
            return Task.FromResult<Product>(null);
        }

        public Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }

        public Task<List<Product>> SearchAsync(string searchTerm)
        {
            var results = _products.Where(p => p.Name.Contains(searchTerm)).ToList();
            return Task.FromResult(results);
        }
    }
}