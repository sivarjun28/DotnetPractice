using Exercise03.Models;

namespace Exercise03.Repository
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
    }
}