namespace Exercise03.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }      // Unique identifier for the product
        public string ProductName { get; set; } // Name of the product
        public int Quantity { get; set; }       // Quantity of the product in the order
        public decimal ProductPrice { get; set; } // Price of the product (at the time of order)

        // Constructor to initialize the order item with product details
        public OrderItem(int productId, string productName, int quantity, decimal productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            ProductPrice = productPrice;
        }
    }
}