using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(int id);
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<ProductResponse>> SearchAsync(ProductSearchCriteria criteria);
    }
}