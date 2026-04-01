using Exercise03.Models.Exercise03.Repository;

namespace Exercise03.Repository
{
    public interface IProductRepository
    {
        Task<bool> ExistsAsync(int productId, CancellationToken ct);
        Task<bool> HasSufficientStockAsync(int productId, int quantity, CancellationToken ct);
        Task<Product> GetProductByIdAsync(int productId, CancellationToken ct);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> products; // In-memory storage for products

        public ProductRepository()
        {
            products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Stock = 50, Price = 999.99m, Description = "High-performance laptop." },
            new Product { Id = 2, Name = "Headphones", Stock = 100, Price = 49.99m, Description = "Noise-cancelling headphones." },
            new Product { Id = 3, Name = "Keyboard", Stock = 25, Price = 19.99m, Description = "Mechanical keyboard with RGB." }
        };
        }

        public async Task<bool> ExistsAsync(int productId, CancellationToken ct)
        {
            return await Task.FromResult(products.Any(p => p.Id == productId));
        }

        public async Task<Product> GetProductByIdAsync(int productId, CancellationToken ct)
        {
          return await Task.FromResult(products.FirstOrDefault(p => p.Id == productId));
        }

        public async Task<bool> HasSufficientStockAsync(int productId, int quantity, CancellationToken ct)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
           return await Task.FromResult(product != null && product.Stock >= quantity);
        }
    }
}