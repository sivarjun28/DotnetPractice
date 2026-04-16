namespace Exercise01.Services
{
    public class InMemoryEventPublisher : IEventPublisher
    {

        private readonly ILogger<InMemoryEventPublisher> _logger;

        public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
        {
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _logger.LogInformation($"Event published: {typeof(TEvent).Name} at {@event.OccurredAt}");

            await Task.CompletedTask;
        }

    }
}