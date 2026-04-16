using Exercise01.Services;

namespace Exercise01.Models
{
    public class ProductCreatedEvent : IEvent
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}
}