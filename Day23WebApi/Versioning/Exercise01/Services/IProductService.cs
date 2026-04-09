using Exercise01.Models;

namespace Exercise01.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product); 
        Task UpdateAsync(Product product);
    }
}