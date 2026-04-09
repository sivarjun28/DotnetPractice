using System.Collections.Generic;
using System.Threading.Tasks;
using ProductApi.Models;
using ProductApi.Models.ProductApi.Models;

namespace ProductApi.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> SearchAsync(string searchTerm);
    }
}