using Exercise01.Models;

namespace Exercise01.Repository
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_products.ToList());
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(product);
        }

        public async Task<Product> AddAsync(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return await Task.FromResult(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null) return null;
            existing.Name = product.Name;
            existing.Price = product.Price;
            return await Task.FromResult(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }
        public async Task<IEnumerable<Product>> SearchAsync(string searchItem)
        {
            var results = _products
                            .Where(p => p.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            return await Task.FromResult(results);
        }
    }
}
