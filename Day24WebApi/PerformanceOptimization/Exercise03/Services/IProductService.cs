using Exercise03.Models;

namespace Exercise03.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
    }

}