namespace Exercise03.Models
{
    public class Order
    {
        public int Id { get; set; }               // Unique identifier for the order
        public string CustomerId { get; set; }    // Unique identifier for the customer who placed the order
        public List<OrderItem> Items { get; set; } = new List<OrderItem>(); // List of items in the order
        public ShippingAddress ShippingAddress { get; set; } = new ShippingAddress(); // Shipping address for the order
        public string PaymentMethod { get; set; } // Payment method used (e.g., CreditCard, PayPal)
        public string? CouponCode { get; set; }   // Optional coupon code applied to the order
        public decimal TotalAmount { get; set; } // Total order amount (calculated)
        public string Status { get; set; }       // Order status (e.g., Pending, Confirmed, Shipped, Delivered)
        public DateTime CreatedAt { get; set; }  // Timestamp when the order was created
        public DateTime? UpdatedAt { get; set; } // Timestamp when the order was last updated
        public DateTime? CancelledAt { get; set; } // Timestamp if order is cancelled
        public DateTime? PaidAt { get; set; }    // Timestamp if order is paid

        // Constructor to initialize the order with required field
    }
}