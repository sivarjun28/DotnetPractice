using System;
using System.Collections.Generic;
using System.Linq;

// Discount Strategy Interface
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
    string GetDescription();
}

// NoDiscount: No discount applied
public class NoDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice;
    }
    
    public string GetDescription()
    {
        return "No discount";
    }
}

// PercentageDiscount: Discount based on a percentage
public class PercentageDiscount : IDiscountStrategy
{
    private readonly decimal percentage;
    
    public PercentageDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100");
        
        this.percentage = percentage;
    }
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice - (originalPrice * (percentage / 100));
    }
    
    public string GetDescription()
    {
        return $"{percentage}% discount";
    }
}

// FixedAmountDiscount: Discount based on a fixed amount
public class FixedAmountDiscount : IDiscountStrategy
{
    private readonly decimal amount;
    
    public FixedAmountDiscount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount must be greater than or equal to 0");
        
        this.amount = amount;
    }
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return Math.Max(0, originalPrice - amount); // Ensure no negative total price
    }
    
    public string GetDescription()
    {
        return $"Fixed discount of {amount:C}";
    }
}

// BulkDiscount: Discount applied when buying in bulk (more than 3 items)
public class BulkDiscount : IDiscountStrategy
{
    private readonly int requiredQuantity;
    private readonly decimal discountPerItem;

    public BulkDiscount(int requiredQuantity, decimal discountPerItem)
    {
        if (requiredQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");
        
        this.requiredQuantity = requiredQuantity;
        this.discountPerItem = discountPerItem;
    }

    public decimal ApplyDiscount(decimal originalPrice)
    {
        int itemCount = 4; // For demonstration, we assume we are always adding 4 items
        if (itemCount >= requiredQuantity)
        {
            return originalPrice - (discountPerItem * itemCount); // Apply discount per item
        }

        return originalPrice;
    }

    public string GetDescription()
    {
        return $"Bulk discount: {discountPerItem:C} off per item for {requiredQuantity}+ items";
    }
}

// SeasonalDiscount: Discount applied during a specific season
public class SeasonalDiscount : IDiscountStrategy
{
    private readonly DateTime startDate;
    private readonly DateTime endDate;
    
    public SeasonalDiscount(DateTime startDate, DateTime endDate)
    {
        this.startDate = startDate;
        this.endDate = endDate;
    }

    public decimal ApplyDiscount(decimal originalPrice)
    {
        DateTime currentDate = DateTime.Now;
        if (currentDate >= startDate && currentDate <= endDate)
        {
            return originalPrice * 0.8m; // Apply 20% discount during the seasonal period
        }

        return originalPrice;
    }

    public string GetDescription()
    {
        return $"Seasonal discount (20%) from {startDate:MMMM dd} to {endDate:MMMM dd}";
    }
}

// LoyaltyDiscount: Discount applied based on customer points
public class LoyaltyDiscount : IDiscountStrategy
{
    private readonly int loyaltyPoints;
    
    public LoyaltyDiscount(int loyaltyPoints)
    {
        if (loyaltyPoints < 0)
            throw new ArgumentException("Loyalty points must be greater than or equal to 0");
        
        this.loyaltyPoints = loyaltyPoints;
    }

    public decimal ApplyDiscount(decimal originalPrice)
    {
        if (loyaltyPoints >= 100) // Discount applied for 100 or more points
        {
            return originalPrice * 0.9m; // Apply 10% discount
        }

        return originalPrice;
    }

    public string GetDescription()
    {
        return loyaltyPoints >= 100 ? "Loyalty discount (10%)" : "No loyalty discount";
    }
}

// ShoppingCart: Uses discount strategies
public class ShoppingCart
{
    private List<decimal> items = new();
    private IDiscountStrategy discountStrategy;
    
    public ShoppingCart(IDiscountStrategy discountStrategy)
    {
        this.discountStrategy = discountStrategy;
    }
    
    public void AddItem(decimal price)
    {
        items.Add(price);
    }
    
    public void SetDiscountStrategy(IDiscountStrategy strategy)
    {
        discountStrategy = strategy;
    }
    
    public decimal GetTotal()
    {
        decimal subtotal = items.Sum();
        return discountStrategy.ApplyDiscount(subtotal);
    }
    
    public void Checkout()
    {
        decimal subtotal = items.Sum();
        decimal total = GetTotal();
        decimal discount = subtotal - total;
        
        Console.WriteLine($"Subtotal: {subtotal:C}");
        Console.WriteLine($"Discount: {discountStrategy.GetDescription()}");
        Console.WriteLine($"Discount Amount: {discount:C}");
        Console.WriteLine($"Total: {total:C}");
    }
}

// Test
public class Program
{
    public static void Main()
    {
        // Create shopping cart with No discount
        ShoppingCart cart = new(new NoDiscount());
        cart.AddItem(100);
        cart.AddItem(200);
        cart.AddItem(150);
        cart.Checkout();
        
        // Apply Percentage discount
        cart.SetDiscountStrategy(new PercentageDiscount(10));
        cart.Checkout();
        
        // Apply Fixed Amount discount
        cart.SetDiscountStrategy(new FixedAmountDiscount(50));
        cart.Checkout();
        
        // Apply Bulk discount
        cart.SetDiscountStrategy(new BulkDiscount(3, 10));
        cart.Checkout();
        
        // Apply Seasonal discount (simulate current season)
        cart.SetDiscountStrategy(new SeasonalDiscount(new DateTime(2023, 12, 1), new DateTime(2023, 12, 31)));
        cart.Checkout();
        
        // Apply Loyalty discount
        cart.SetDiscountStrategy(new LoyaltyDiscount(120));
        cart.Checkout();
    }
}