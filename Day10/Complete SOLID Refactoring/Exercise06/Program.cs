using System;
using System.Collections.Generic;


/*
Refactor applying ALL SOLID principles:

S - Separate concerns:
    - Order (model)
    - Customer (model)
    - Product (model)
    - OrderValidator
    - DiscountCalculator
    - InventoryManager
    - InvoiceGenerator

O - Open/Closed:
    - IPaymentProcessor with multiple implementations
    - IDiscountStrategy

L - Liskov Substitution:
    - Ensure all implementations are substitutable

I - Interface Segregation:
    - IEmailSender
    - IOrderRepository
    - IInventoryRepository

D - Dependency Inversion:
    - OrderService depends on abstractions
    - Use dependency injection
*/
// Models (SRP)
public class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }  // Changed from string to decimal
}

public class OrderItem
{
    public Product Product { get; set; }  // Capitalized 'Product'
    public int Quantity { get; set; }
}

public class Order
{
    public Customer Customer { get; set; }  // Capitalized 'Customer'
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
}

// Interfaces (OCP)
public interface IPaymentProcessor
{
    bool ProcessPayment(Order order);
}

public class CreditcardPayment : IPaymentProcessor
{
    public bool ProcessPayment(Order order)
    {
        Console.WriteLine("Processing Creditcard payment");
        return true;
    }
}

public class PaypalPayment : IPaymentProcessor
{
    public bool ProcessPayment(Order order)
    {
        Console.WriteLine("Processing Paypal payment");
        return true;
    }
}

public interface IDiscountStrategy
{
    decimal CalculateDiscount(Order order);
}

public class PercentageDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(Order order)
    {
        return order.TotalAmount * 0.1m;
    }
}

public class FixedDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(Order order)
    {
        return 20m;
    }
}

// Interface Segregation Principle (ISP)
public interface IEmailSender
{
    void SendEmail(string email, string subject, string body);
}

public interface IOrderRepository
{
    void Save(Order order);
}

public interface IInventoryService
{
    void CheckInventory(Order order);
}

public class InMemoryOrderRepository : IOrderRepository
{
    private List<Order> _orders = new List<Order>();

    public void Save(Order order)
    {
        _orders.Add(order);
        Console.WriteLine($"Order saved: {order.Customer.Name} - Total: {order.TotalAmount}");
    }

    public List<Order> GetAllOrders()
    {
        return _orders;
    }
}

// Concrete implementation of IEmailSender
public class EmailSender : IEmailSender
{
    public void SendEmail(string email, string subject, string body)
    {
        Console.WriteLine($"Sending email to: {email}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
    }
}

// Concrete implementation of IInventoryService
public class InventoryService : IInventoryService
{
    public void CheckInventory(Order order)
    {
        foreach (var orderItem in order.Items)
        {
            Console.WriteLine($"Checking inventory for {orderItem.Product.Name}... Available.");
        }
    }
}

// Dependency Inversion Principle (DIP)
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IEmailSender _emailSender;
    private readonly IInventoryService _inventoryService;
    private readonly IDiscountStrategy _discountStrategy;

    public OrderService(IOrderRepository orderRepository, IPaymentProcessor paymentProcessor, IEmailSender emailSender,
                        IInventoryService inventoryService, IDiscountStrategy discountStrategy)
    {
        _orderRepository = orderRepository;
        _paymentProcessor = paymentProcessor;
        _emailSender = emailSender;
        _inventoryService = inventoryService;
        _discountStrategy = discountStrategy;
    }

    public void PlaceOrder(Order order)
    {
        if (order.Items == null || order.Items.Count == 0)
        {
            Console.WriteLine("No items in the order. Cannot proceed.");
            return;
        }

        _inventoryService.CheckInventory(order);

        decimal discount = _discountStrategy.CalculateDiscount(order);
        order.TotalAmount -= discount;

        if (order.TotalAmount <= 0)
        {
            Console.WriteLine("Total amount after discount is zero or less. Order not processed.");
            return;
        }

        bool isPaid = _paymentProcessor.ProcessPayment(order);
        if (!isPaid)
        {
            Console.WriteLine("Payment Failed");
            return;
        }

        _orderRepository.Save(order);
        _emailSender.SendEmail(order.Customer.Email, "Order Confirmation", "Your order has been placed successfully.");
        Console.WriteLine("Order placed successfully.");
    }
}

// Main Program
public class Program
{
    public static void Main(string[] args)
    {
        // Create dependencies (using CreditCardPayment)
        IPaymentProcessor paymentProcessor = new CreditcardPayment();
        IOrderRepository orderRepo = new InMemoryOrderRepository();
        IEmailSender emailSender = new EmailSender();
        IInventoryService inventoryService = new InventoryService();
        IDiscountStrategy discountStrategy = new PercentageDiscount();

        var orderService = new OrderService(
            orderRepo,
            paymentProcessor,
            emailSender,
            inventoryService,
            discountStrategy
        );

        // Create an order
        var order = new Order
        {
            Customer = new Customer { Name = "arjun", Email = "arjun@gmail.com" },
            Items = new List<OrderItem>
            {
                new OrderItem { Product = new Product { Name = "Laptop", Price = 1000 }, Quantity = 1 }
            },
            TotalAmount = 1000
        };

        // Place the order
        orderService.PlaceOrder(order);
    }
}