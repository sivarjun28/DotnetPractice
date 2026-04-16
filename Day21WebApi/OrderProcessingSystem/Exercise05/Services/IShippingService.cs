using Exercise05.Models;

namespace Exercise05.Services
{
    public interface IShippingService
    {
        Task<ShippingLabel> CreateLabelAsync(ShippingAddress address, List<OrderItem> items);
        Task CancelShipmentAsync(string shipmentId);
    }
}