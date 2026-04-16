using Exercise05.Models;

namespace Exercise05.Services
{
    public class ShippingService : IShippingService
    {
        public Task CancelShipmentAsync(string shipmentId)
        {
            Console.WriteLine($"Shipment {shipmentId} cancelled.");
            return Task.CompletedTask;
        }

        public Task<ShippingLabel> CreateLabelAsync(ShippingAddress address, List<OrderItem> items)
        {
            var shipmentId = Guid.NewGuid().ToString();
            return Task.FromResult(new ShippingLabel
            {
                ShipmentId = shipmentId,
                TrackingNumber = $"TRACK-{shipmentId[..8]}"
            });
        }
    }
}