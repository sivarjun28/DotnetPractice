using Exercise03.Models;
using Exercise03.Repository;

namespace Exercise03.Services
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepo;

        public ProductService(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepo.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepo.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await _productRepo.AddAsync(product);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            return await _productRepo.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepo.DeleteAsync(id);
        }
    }
}