using Exercise05.Models;
using Exercise05.Services;

namespace Exercise05.Processors
{
    public class OrderProcessor : IOrderProcessor
    {
        private readonly IInventoryService _inventory;
        private readonly IPaymentService _payment;
        private readonly IShippingService _shipping;
        private readonly INotificationService _notification;
        private readonly ILogger<OrderProcessor> _logger;

        public OrderProcessor(
            IInventoryService inventory,
            IPaymentService payment,
            IShippingService shipping,
            INotificationService notification,
            ILogger<OrderProcessor> logger)
        {
            _inventory = inventory;
            _payment = payment;
            _shipping = shipping;
            _notification = notification;
            _logger = logger;
        }

        public async Task<OrderResult> ProcessOrderAsync(Order order)
        {
            var reservedProducts = new List<OrderItem>();
            PaymentResult paymentResult = null;
            ShippingLabel shippingLabel = null;

            try
            {
                foreach (var item in order.Items)
                {
                    if (!await _inventory.CheckAvailabilityAsync(item.ProductId, item.Quantity))
                        return new OrderResult { Success = false, Message = $"Product {item.ProductId} out of stock" };
                }
                paymentResult = await _payment.ProcessPaymentAsync(order.Payment);
                if (!paymentResult.Success)
                {
                    return new OrderResult { Success = false, Message = paymentResult.ErrorMessage };
                }

                foreach (var item in order.Items)
                {
                    await _inventory.ReserveAsync(item.ProductId, item.Quantity);
                    reservedProducts.Add(item);
                }
                shippingLabel = await _shipping.CreateLabelAsync(order.Address, order.Items);
                await _notification.SendOrderConfirmationAsync(order);
                return new OrderResult { Success = true, Message = "Order processed successfully" };
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "Error processing order");

                // Compensating actions
                if (shippingLabel != null) await _shipping.CancelShipmentAsync(shippingLabel.ShipmentId);
                foreach (var item in reservedProducts) await _inventory.ReleaseAsync(item.ProductId, item.Quantity);
                if (paymentResult?.Success == true) await _payment.RefundAsync(paymentResult.TransactionId);

                return new OrderResult { Success = false, Message = "Order processing failed" };
            }
        }
    }
}
