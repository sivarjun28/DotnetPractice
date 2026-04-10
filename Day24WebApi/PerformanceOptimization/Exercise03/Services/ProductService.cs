using Exercise03.Models;
using Exercise03.Repository;

namespace Exercise03.Services
{
    public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repo.GetAllOptimizedAsync();
    }
}
}