using Exercise01.Services;

namespace Exercise01.Models
{
    public class ProductUpdatedEvent : IEvent
    {
        public int ProductId { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}