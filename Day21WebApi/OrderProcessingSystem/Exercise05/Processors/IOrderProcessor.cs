using Exercise05.Models;

namespace Exercise05.Processors
{
    public interface IOrderProcessor
    {
        Task<OrderResult> ProcessOrderAsync(Order order);
    }
}

