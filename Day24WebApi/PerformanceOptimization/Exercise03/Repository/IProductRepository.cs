using Exercise03.Models;

namespace Exercise03.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllOptimizedAsync();
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Product>> SearchOptimizedAsync(ProductSearchCriteria criteria);
        Task BulkInsertAsync(List<Product> products);
    }
}