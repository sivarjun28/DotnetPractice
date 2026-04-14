using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<Product>> SearchAsync(ProductSearchCriteria criteria);
        Task<bool> SkuExistsAsync(string sku);
    }
}