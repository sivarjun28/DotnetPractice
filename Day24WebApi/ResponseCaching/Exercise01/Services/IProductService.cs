using Exercise01.Models;

namespace Exercise01.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string q, string ? category); 
        Task<Product> CreateAsync(CreateProductRequest product);
        Task<Product> UpdateAsync(int id, UpdateProductDto dto);
        Task<Product> DeleteAsync(int id);
    }
}