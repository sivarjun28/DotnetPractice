// See https://aka.ms/new-console-template for more information
// Single Responsibility Principle (SRP)
//The Principle
// A class should have only one reason to change.

// Each class should have one job, one responsibility.
// BAD: This class has multiple responsibilities

// Usage
PaymentProcessor1 processor = new();
processor.ProcessPayment(new CreditCardPayment(), 99.99m);
processor.ProcessPayment(new BitcoinPayment(), 99.99m);

// This violates LSP - behavior is unexpected
void TestRectangle(Rectangle rect)
{
    rect.Width = 5;
    rect.Height = 10;
    
    // Expected: 50, but Square will give 100!
    Console.WriteLine($"Area: {rect.CalculateArea()}");
}

Rectangle rectangle = new Rectangle();
TestRectangle(rectangle);  // Area: 50 ✓

Rectangle square = new Square();
TestRectangle(square);     // Area: 100 ✗ Unexpected!

// No inheritance issues - each shape behaves correctly
void TestShape(IShape shape)
{
    Console.WriteLine($"Area: {shape.CalculateArea()}");
}
TestShape(new Rectangle1(){Height = 5, Width = 5});
TestShape(new Square1(){Side= 4});

// Usage - can easily switch implementations
UserService1 service1 = new UserService1(new SqlDatabase1());
UserService1 service2 = new UserService1(new MongoDatabase());

service1.RegisterUser("Alice");  // Saves to SQL
service2.RegisterUser("Bob");    // Saves to MongoDB


public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Responsibility 1: Data validation
    public bool Validate()
    {
        return !string.IsNullOrEmpty(Name) && Email.Contains('@');
    }
    
    // Responsibility 2: Database operations
    public void Save()
    {
        // Database save logic
        Console.WriteLine($"Saving user to database: {Name}");
    }
    
    // Responsibility 3: Email operations
    public void SendWelcomeEmail()
    {
        Console.WriteLine($"Sending welcome email to {Email}");
    }
    
    // Responsibility 4: Reporting
    public string GenerateReport()
    {
        return $"User Report: {Name} ({Email})";
    }
}

//Single resposiubility

//Res1 Data model
public class User1
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

//Res2 Validation

public class User1Validation
{
    public bool validate(User1 user )
    {
        return !string.IsNullOrWhiteSpace(user.Name) && user.Email.Contains("@");
    }
}

// Responsibility 3: Database operations
public class UserRepository
{
    public void Save(User user)
    {
        Console.WriteLine($"Saving user to database: {user.Name}");
    }
    
    public User? GetById(int id)
    {
        // Retrieve from database
        return null;
    }
}

// Responsibility 4: Email operations
public class EmailService
{
    public void SendWelcomeEmail(User user)
    {
        Console.WriteLine($"Sending welcome email to {user.Email}");
    }
}

// Responsibility 5: Reporting
public class UserReportGenerator
{
    public string GenerateReport(User user)
    {
        return $"User Report: {user.Name} ({user.Email})";
    }
}


//Open/Closed Principle (OCP)
        //Classes should be open for extension but closed for modification.

// BAD: Need to modify class to add new payment methods
public class PaymentProcessor
{
    public void ProcessPayment(string paymentType, decimal amount)
    {
        if (paymentType == "CreditCard")
        {
            Console.WriteLine($"Processing credit card payment: {amount:C}");
        }
        else if (paymentType == "PayPal")
        {
            Console.WriteLine($"Processing PayPal payment: {amount:C}");
        }
        else if (paymentType == "Bitcoin")  // Need to modify for new type
        {
            Console.WriteLine($"Processing Bitcoin payment: {amount:C}");
        }
    }
}

// GOOD: Can add new payment methods without modifying existing code

public interface IPaymentMethod
{
    void ProcessPayment(decimal amount);
}

public class CreditCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment: {amount:C}");
    }
}

public class PayPalPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment: {amount:C}");
    }
}

// New payment method - no modification to existing code
public class BitcoinPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Bitcoin payment: {amount:C}");
    }
}

public class PaypalPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        System.Console.WriteLine($"Processing Paypal Payment {amount:C}");
    }
}

public class PaymentProcessor1
{
    public void ProcessPayment(IPaymentMethod paymentMethod, decimal amount)
    {
        paymentMethod.ProcessPayment(amount);
    }
}

//Liskov Substitution Principle
//Derived classes must be substitutable for their base classes.

// BAD: Square changes behavior of Rectangle
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    
    public int CalculateArea()
    {
        return Width * Height;
    }
}

public class Square : Rectangle
{
    private int side;
    
    public override int Width
    {
        get => side;
        set
        {
            side = value;
            // Problem: Setting width also sets height
        }
    }
    
    public override int Height
    {
        get => side;
        set
        {
            side = value;
            // Problem: Setting height also sets width
        }
    }
}

// GOOD: Use composition or separate interfaces

public interface IShape
{
    int CalculateArea();
}

public class Rectangle1 : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public int CalculateArea()
    {
        return Width * Height;
    }
}

public class Square1 : IShape
{
    public int Side { get; set; }
    
    public int CalculateArea()
    {
        return Side * Side;
    }
}

//Interface Segregation Principle
//Clients should not be forced to depend on interfaces they don't use.

// Many specific interfaces are better than one general-purpose interface.


// BAD: Forcing all implementations to implement unused methods
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void GetSalary();
}

public class Human : IWorker
{
    public void Work() { Console.WriteLine("Working"); }
    public void Eat() { Console.WriteLine("Eating"); }
    public void Sleep() { Console.WriteLine("Sleeping"); }
    public void GetSalary() { Console.WriteLine("Getting salary"); }
}

public class Robot : IWorker
{
    public void Work() { Console.WriteLine("Working"); }
    
    // Robots don't eat or sleep!
    public void Eat() { throw new NotImplementedException(); }
    public void Sleep() { throw new NotImplementedException(); }
    public void GetSalary() { throw new NotImplementedException(); }
}

// GOOD: Small, focused interfaces

public interface IWorkable
{
    void Work();
}

public interface IFeedable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public interface IPayable
{
    void GetSalary();
}

// Human implements all interfaces
public class Human1 : IWorkable, IFeedable, ISleepable, IPayable
{
    public void Work() { Console.WriteLine("Working"); }
    public void Eat() { Console.WriteLine("Eating"); }
    public void Sleep() { Console.WriteLine("Sleeping"); }
    public void GetSalary() { Console.WriteLine("Getting salary"); }
}

// Robot only implements what it needs
public class Robot1 : IWorkable
{
    public void Work() { Console.WriteLine("Working"); }
}


// Dependency Inversion Principle (DIP) 
//High-level modules should not depend on low-level modules. Both should depend on abstraction

// BAD: High-level class depends on low-level implementations

public class SqlDatabase
{
    public void Save(string data)
    {
        Console.WriteLine($"Saving to SQL: {data}");
    }
}

public class UserService
{
    private SqlDatabase database = new();  // Tight coupling!
    
    public void RegisterUser(string username)
    {
        // Business logic
        database.Save(username);  // Depends on concrete class
    }
}

// Problem: Can't switch to different database without modifying UserService

public interface IDatabase
{
    void Save(string data);
    string Load(int id);
}

public class SqlDatabase1 : IDatabase
{
    public string Load(int id)
    {
        return $"Data Fro sql {id}";
    }

    public void Save(string data)
    {
        System.Console.WriteLine($"Saving to sql: {data}");
    }
    
}
public class MongoDatabase : IDatabase
{
    public string Load(int id)
    {
        return $"Data From Momgo {id}";
    }

    public void Save(string data)
    {
        System.Console.WriteLine($"Saving to Mongo: {data}");
    }
    
}

public class UserService1
{
    private readonly IDatabase database;
    public UserService1(IDatabase database)
    {
        this.database = database;
    }

    public void RegisterUser(string username)
    {
        // Business logic
        database.Save(username);  // Works with any IDatabase
    }
}


// APPLYING ALL SOLID PRINCIPLES

// SRP: Separate data model
public class Order
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    
}

// SRP: Separate validation
public class OrderValidator
{
    public bool Validate(Order order)
    {
        return order.Items.Count > 0 && order.Total > 0;
    }
}

// OCP & DIP: Payment abstraction
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
}

// OCP: Can add new implementations without modifying existing code
public class StripePayment : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing via Stripe: {amount:C}");
        return true;
    }
}

// DIP: Repository abstraction
public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(int id);
}

// ISP: Separate notification concerns
public interface IEmailNotifier
{
    void SendEmail(string to, string subject, string body);
}

// DIP: OrderService depends on abstractions
public class OrderService
{
    private readonly IOrderRepository repository;
    private readonly IPaymentProcessor paymentProcessor;
    private readonly IEmailNotifier emailNotifier;
    
    // Dependency injection
    public OrderService(
        IOrderRepository repository,
        IPaymentProcessor paymentProcessor,
        IEmailNotifier emailNotifier)
    {
        this.repository = repository;
        this.paymentProcessor = paymentProcessor;
        this.emailNotifier = emailNotifier;
    }
    
    public bool PlaceOrder(Order order)
    {
        // SRP: Validation is separate
        OrderValidator validator = new();
        if (!validator.Validate(order))
            return false;
        
        // Process payment
        if (!paymentProcessor.ProcessPayment(order.Total))
            return false;
        
        // Save order
        repository.Save(order);
        
        // Send notification
        emailNotifier.SendEmail("customer@example.com", "Order Confirmed", "Thank you!");
        
        return true;
    }
}
