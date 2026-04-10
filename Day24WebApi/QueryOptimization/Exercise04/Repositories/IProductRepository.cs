using Exercise04.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise04.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductListDto>> GetProductListAsync();
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize);
        Task<Product?> GetByIdCompiledAsync(int id);
    }
}