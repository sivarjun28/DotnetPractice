namespace ECommerceAPI.Models.Entities
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
    }
}