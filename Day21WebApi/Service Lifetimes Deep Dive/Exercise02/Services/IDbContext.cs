using Exercise02.Models;

namespace Exercise02.Services
{
    public interface IDbContext
    {
        Task<List<Order>> GetPendingOrdersAsync();
        Task MarkOrderProcessedAsync(Order order);
    }

    public class DbContextStub : IDbContext
    {
        private readonly List<Order> _orders = new()
    {
        new Order { Id = 1, Description = "Order 1" },
        new Order { Id = 2, Description = "Order 2" }
    };

        public Task<List<Order>> GetPendingOrdersAsync()
        {
           
            return Task.FromResult(_orders.ToList());
        }

        public Task MarkOrderProcessedAsync(Order order)
        {
            _orders.Remove(order);
            return Task.CompletedTask;
        }
    }

}