using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
namespace Exercise05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //create Products
            var mobile = new Product(1,"Samsung", 20000.98m, "Electronis");
            var carToy = new Product(2,"Toy", 25000.98m, "Electronis");
            var laptop = new Product(3,"macbook", 100000.98m, "Electronis");

            var shoppingCart = new ShoppingCart();
            shoppingCart.AddItem(mobile,2);
            shoppingCart.AddItem(carToy,4);
            shoppingCart.AddItem(laptop,8);

            shoppingCart.DisplayCartSummary();

            shoppingCart.ApplyDiscount("FLAT30");
            shoppingCart.DisplayCartSummary();
            shoppingCart.UpdateQuantity(1,20);

            shoppingCart.DisplayCartSummary();

            shoppingCart.ClearCart();
        }
    }

    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Category { get; private set; }

        public Product(int id, string name, decimal price, string category)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
        }

    }

    public class CartItem
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public decimal SubTotal => Product.Price * Quantity;

        public CartItem(Product product, int quantity)
        {
            if (quantity < 1) throw new ArgumentException("Quantity Cannot be Negative");
            if (quantity > 99) throw new ArgumentException("Quantity Cannot exceed 99");

            Product = product;
            Quantity = quantity;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 1) throw new ArgumentException("Quantity Cannot be Negative");
            if (newQuantity > 99) throw new ArgumentException("Quantity Cannot exceed 99");
            Quantity = newQuantity;
        }
    }
    //Shopping cart class
    public class ShoppingCart
    {
        private List<CartItem> _cartItems;
        private decimal _total;
        private decimal _discount;
        private bool _discountApplied;

        public ShoppingCart()
        {
            _cartItems = new List<CartItem>();
            _total = 0m;
            _discount = 0m;
            _discountApplied = false;
        }

        //Add item to the cart
        public void AddItem(Product product, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if(existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                _cartItems.Add(new CartItem(product, quantity));
            }
        }

        // Remove item from the cart
        public void RemoveItem(int productId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Product.Id == productId);
            if(item != null)
            {
                _cartItems.Remove(item);
            }
        }
        //upfate Item quantity
        public void UpdateQuantity(int productId, int newQuantity)
        {
            var item = _cartItems.FirstOrDefault(i => i.Product.Id == productId);

            if(item != null)
            {
                item.UpdateQuantity(newQuantity);
            }
        }

        //Calculate the Total price before Discount
        public decimal CalculateTotal()
        {
            _total = _cartItems.Sum(item => item.SubTotal);
            return _total;
        }

          // Apply discount code
        public void ApplyDiscount(string discountCode)
        {
            if (_discountApplied)
            {
                System.Console.WriteLine("Discount Already Applied");
            }
            if(discountCode.Equals("DISCOUNT10", StringComparison.OrdinalIgnoreCase))
            {
                _discount = CalculateTotal() * 0.10m;
            }
            else if(discountCode.Equals("FLAT30", StringComparison.OrdinalIgnoreCase))
            {
                _discount = 30m;
            }
            else
            {
                System.Console.WriteLine("Ivalid discount code");
                return;
            }
            _discountApplied = true;
            System.Console.WriteLine($"Discount applied: {_discount:C}");
        }

        //clear all the items in the cart
        public void ClearCart()
        {
            _cartItems.Clear();
            _total = 0m;
            _discount = 0m;
            _discountApplied = false;
            System.Console.WriteLine("Card has been cleared");
        }

        public void DisplayCartSummary()
        {
            CalculateTotal();
            System.Console.WriteLine("Cart Summary");
            foreach(var item in _cartItems)
            {
                System.Console.WriteLine($"{item.Product.Name} x {item.Quantity} : {item.SubTotal:C}");
            }
            decimal totalAfterDiscount = _total - _discount;
            System.Console.WriteLine($"Total (before Discount): {_total:C}");
            System.Console.WriteLine($"Discount: {_discount:C}");
            System.Console.WriteLine($"Total (after discount): {totalAfterDiscount:C}");
        }
    
    }
}