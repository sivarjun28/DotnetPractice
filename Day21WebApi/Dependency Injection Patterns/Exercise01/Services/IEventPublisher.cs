namespace Exercise01.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
    public interface IEvent
    {
        DateTime OccurredAt { get; }
    }

}