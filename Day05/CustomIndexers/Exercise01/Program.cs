using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShoppingCart shoppingCart = new();
            
            shoppingCart.AddItem("P0001", "Laptop", 99999.9m, 1);  // Laptop - ProductId: P0001
            shoppingCart.AddItem("P0002", "Mobile", 9999.9m, 1);   // Mobile - ProductId: P0002
            shoppingCart.AddItem("P0003", "Mouse", 999.9m, 1);     // Mouse - ProductId: P0003
            shoppingCart.DisplayCart();

            // Check quantity of Laptop (P0001)
            System.Console.WriteLine($"\nLaptop Quantity: {shoppingCart["P0001"]?.Quantity}");

            // Update Mobile quantity (ProductId: P0002)
            if (shoppingCart["P0002"] is CartItem mobilePhone)
            {
                mobilePhone.Quantity = 3;  // Update Mobile quantity
            }
            shoppingCart.RemoveItem("P0003");
            shoppingCart.DisplayCart();
        }
    }

    public class CartItem
    {
        public string ProductId { get; init; }
        public string ProductName { get; init; }
        public int Quantity { get; set; }
        public decimal Price { get; init; }

        public decimal Subtotal => Quantity * Price;

        public override string ToString()
        {
            return $"{ProductName} x {Quantity} @ {Price:C} = {Subtotal:C}";
        }
    }

    public class ShoppingCart
    {
        private Dictionary<string, CartItem> items = new(); // Dictionary should map productId to CartItem
        
        // Indexer for accessing CartItem by ProductId
        public CartItem? this[string productId]
        {
            get
            {
                if (items.TryGetValue(productId, out var cartItem))
                    return cartItem;
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (items.ContainsKey(productId))
                    {
                        // Update the existing item's quantity
                        items[productId].Quantity += value.Quantity;
                    }
                    else
                    {
                        // Add new item to the dictionary
                        items[productId] = value;
                    }
                }
            }
        }

        // Add item to the cart
        public void AddItem(string productId, string productName, decimal price, int quantity = 1)
        {
            if (items.ContainsKey(productId))
            {
                items[productId].Quantity += quantity;
            }
            else
            {
                items[productId] = new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity
                };
            }
        }

        // Remove item from the cart
        public void RemoveItem(string productId)
        {
            if (items.ContainsKey(productId))
            {
                items.Remove(productId);
            }
        }

        // Get the total price of the cart
        public decimal GetTotal()
        {
            return items.Values.Sum(item => item.Subtotal);
        }

        // Clear all items in the cart
        public void Clear()
        {
            items.Clear();
        }

        // Display the contents of the shopping cart
        public void DisplayCart()
        {
            System.Console.WriteLine("=== Shopping Cart ===");
            if (items.Count == 0)
            {
                System.Console.WriteLine("Cart is empty");
                return;
            }
            foreach (var item in items.Values)
            {
                System.Console.WriteLine(item);
            }
            System.Console.WriteLine($"\nTotal: {GetTotal():C}");
        }
    }
}