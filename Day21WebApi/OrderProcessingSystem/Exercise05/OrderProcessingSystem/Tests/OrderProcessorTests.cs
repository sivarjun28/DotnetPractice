using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercise05.Processors;
using Exercise05.Models;
using Exercise05.Services;

namespace OrderProcessingSystem.Tests
{
    public class OrderProcessorTests
    {
        private readonly Mock<IInventoryService> _mockInventory;
        private readonly Mock<IPaymentService> _mockPayment;
        private readonly Mock<IShippingService> _mockShipping;
        private readonly Mock<INotificationService> _mockNotification;
        private readonly ILogger<OrderProcessor> _logger;

        public OrderProcessorTests()
        {
            _mockInventory = new Mock<IInventoryService>();
            _mockPayment = new Mock<IPaymentService>();
            _mockShipping = new Mock<IShippingService>();
            _mockNotification = new Mock<INotificationService>();
            _logger = new LoggerFactory().CreateLogger<OrderProcessor>();
        }

        [Fact]
        public async Task ProcessOrderAsync_ShouldReturnSuccess_WhenEverythingWorks()
        {
            var order = new Order
            {
                OrderId = 1,
                Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 2 } },
                Payment = new PaymentRequest { Amount = 100, CreditCardNumber = "1234" },
                Address = new ShippingAddress { Street = "Main St", City = "NY", Zip = "10001", Country = "USA" }
            };

            _mockInventory.Setup(i => i.CheckAvailabilityAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockPayment.Setup(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                        .ReturnsAsync(new PaymentResult { Success = true, TransactionId = "txn1" });
            _mockShipping.Setup(s => s.CreateLabelAsync(It.IsAny<ShippingAddress>(), It.IsAny<List<OrderItem>>()))
                         .ReturnsAsync(new ShippingLabel { ShipmentId = "ship1", TrackingNumber = "track1" });

            var processor = new OrderProcessor(
                _mockInventory.Object,
                _mockPayment.Object,
                _mockShipping.Object,
                _mockNotification.Object,
                _logger
            );

            var result = await processor.ProcessOrderAsync(order);

            Assert.True(result.Success);
            Assert.Equal("Order processed successfully", result.Message);
        }

        [Fact]
        public async Task ProcessOrderAsync_ShouldFail_WhenInventoryNotAvailable()
        {
            var order = new Order
            {
                OrderId = 2,
                Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 5 } },
                Payment = new PaymentRequest { Amount = 100, CreditCardNumber = "1234" }
            };

            _mockInventory.Setup(i => i.CheckAvailabilityAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var processor = new OrderProcessor(
                _mockInventory.Object,
                _mockPayment.Object,
                _mockShipping.Object,
                _mockNotification.Object,
                _logger
            );

            var result = await processor.ProcessOrderAsync(order);

            Assert.False(result.Success);
            Assert.Contains("out of stock", result.Message);
        }
    }
}