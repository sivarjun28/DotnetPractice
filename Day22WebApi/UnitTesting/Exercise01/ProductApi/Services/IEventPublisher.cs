using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync(ProductCreatedEvent eventMessage);
    }
}