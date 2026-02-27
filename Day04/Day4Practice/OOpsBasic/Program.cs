using System;
using System.CodeDom.Compiler;

namespace OOpsBasic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create instances of the Person class
            Person p = new Person("Siva", 23);
            Person p1 = new Person("Arjun", 23);

            // Call the Introduce method for both objects
            p.Introduce();
            p1.Introduce();

            BankAccount acc1 = new BankAccount();
            BankAccount acc2 = new BankAccount("Arjun");
            BankAccount acc3 = new BankAccount("Arjuna", 1000);
            acc1.DisplayAccountDetails();
            acc2.DisplayAccountDetails();
            acc3.DisplayAccountDetails();

            // Traditional way
            Person1 p3 = new Person1();
            p3.Name = "Shiva";
            string name = p3.Name;

            Person1 p4 = new Person1
            {
                Name = "Arjun"
            };

            // Target-typed new (C# 9+)
            Person1 person = new()
            {
                Name = "Krishna"
            };

            var account = new BankAccount2();
            account.Deposit(1000);
            account.Withdraw(500);
            Console.WriteLine(account.Balance);

            EmailService email = new EmailService();
            email.SendEmail("user@example.com", "Hello", "Message body");

            System.Console.WriteLine(Counter.GetTotalInstances());
            Counter c1 = new Counter();
            Counter c2 = new Counter();
            Counter c3 = new Counter();

            c1.ShowInstanceNumber();
            c2.ShowInstanceNumber();

            System.Console.WriteLine(Counter.GetTotalInstances());

            double area = MathHelper.CircleArea(5);
            double CircleCircumference = MathHelper.CircleCircumference(5);
            System.Console.WriteLine(CircleCircumference);
            System.Console.WriteLine(area);

            var c = new Configuration();
            System.Console.WriteLine(c.InstanceId);
            System.Console.WriteLine(Configuration.TaxRate);
            // Configuration.TaxRate = 67;

        }
    }

    public class BankAccount
    {
        private string accountNumber;
        private decimal balance;
        private string ownerName;

        // Default constructor
        public BankAccount()
        {
            accountNumber = GenerateAccountNumber();
            balance = 0;
            ownerName = "Unknown";
        }

        // Parameterized constructor
        public BankAccount(string owner)
        {
            accountNumber = GenerateAccountNumber();
            balance = 0;
            ownerName = owner;
        }

        // Full constructor
        public BankAccount(string owner, decimal initialBalance)
        {
            accountNumber = GenerateAccountNumber();
            balance = initialBalance;
            ownerName = owner;
        }

        // Constructor chaining (calling another constructor)
        public BankAccount(string owner, decimal initialBalance, string accountNum)
            : this(owner, initialBalance)  // Calls the constructor above
        {
            accountNumber = accountNum;
        }

        private string GenerateAccountNumber()
        {
            return $"ACC{Random.Shared.Next(100000, 999999)}";
        }
        public void DisplayAccountDetails()
        {
            Console.WriteLine($"Account Number: {accountNumber}, Owner: {ownerName}, Balance: {balance:C}");
        }
    }

    //Properties are the C# way of getters/setters
    public class Person1
    {
        public string Name { get; set; }
        public int Age { get; set; }



    }

    //Property Variations

    public class Person2
    {
        public string Name { get; set; }

        public string Id { get; init; }

        public int Age { get; set; }

        public int BirthYear => DateTime.Now.Year - Age;

        // Property with backing field (when you need validation)
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }

            set
            {
                if (value < 0)

                    throw new ArgumentException("Balance Cannot be zero");
                _balance = value;
            }

        }
        // Expression-bodied property accessors (C# 7+)
        // Expression-bodied property accessors (C# 7+)
        private string _name;
        public string Name1
        {
            get => _name;
            set => _name = value?.Trim() ?? throw new ArgumentNullException();
        }

        // Private setter (read-only from outside)
        public int LoginCount { get; private set; }

        public void Login()
        {
            LoginCount++;  // Can modify inside class
        }
    }

    //Properties vs Fields

    public class Product
    {
        // FIELD - direct storage (typically private)
        private decimal _cost;

        // PROPERTY - controlled access
        public decimal Price
        {
            get => _cost * 1.2m;  // Add 20% markup
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be positive");
                _cost = value / 1.2m;
            }
        }

        // Auto-property (compiler creates hidden field)
        public string Name { get; set; }
    }



    //Part 3: Encapsulation & Access Modifiers
    public class BankAccount1
    {
        // PUBLIC - accessible from anywhere
        public string AccountNumber { get; set; }

        // PRIVATE - only accessible within this class
        private decimal balance;

        // PROTECTED - accessible in this class and derived classes
        protected string internalId;

        // INTERNAL - accessible within same assembly (project)
        internal void ProcessTransaction() { }

        // PROTECTED INTERNAL - accessible in same assembly OR derived classes
        protected internal void AuditLog() { }

        // PRIVATE PROTECTED (C# 7.2+) - accessible in derived classes in same assembly
        private protected void InternalAudit() { }
    }

    // Default access modifiers
    class MyClass { }  // internal (default for classes)
    public class MyPublicClass
    {
        int field;  // private (default for members)
    }

    // ✅ GOOD - Encapsulation with properties and methods
    public class BankAccount2
    {
        private decimal balance;
        public decimal Balance => balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Diposit Must be Positive");

            }
            balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdraw amount must be positive");
            }
            if (amount > balance)
            {
                return false;
            }
            balance -= amount;
            return true;
        }
    }


    //Data Hiding & Information Hiding
    public class EmailService
    {
        // IMPLEMENTATION DETAIL (private)
        private string smtpServer = "smtp.gmail.com";
        private int port = 587;

        // PUBLIC INTERFACE
        public void SendEmail(string to, string subject, string body)
        {
            // Users don't need to know about SMTP details
            ConnectToServer();
            AuthenticateUser();
            SendMessage(to, subject, body);
            DisconnectFromServer();
        }

        // PRIVATE IMPLEMENTATION
        private void ConnectToServer() { /* ... */ }
        private void AuthenticateUser() { /* ... */ }
        private void SendMessage(string to, string subject, string body) { /* ... */ }
        private void DisconnectFromServer() { /* ... */ }
    }

    //static members 
    public class Counter
    {
        private int instanceCount;
        private static int totalInstances = 0;

        public Counter()
        {
            totalInstances++;
            instanceCount = totalInstances;
        }

        public void ShowInstanceNumber()
        {
            System.Console.WriteLine($"iam instance number {instanceCount}");
        }

        public static int GetTotalInstances()
        {
            return totalInstances;
        }
    }

    public static class MathHelper
    {
        public static double Pi = 3.14159;

        public static double CircleArea(double radius)
        {
            return Pi * radius * radius;
        }

        public static double CircleCircumference(double radius)
        {
            return 2 * Pi * radius;
        }
    }
//Constants and Readonly
public class Configuration
{
    public const double TaxRate = 0.08;       // Constant value for tax rate
    public const string appName = "My App";   // Constant string for app name

    public readonly string InstanceId;        // Instance-level readonly field
    public static readonly DateTime StartTime; // Static readonly field

    // Static constructor (called once when the class is first used)
    static Configuration()
    {
        StartTime = DateTime.Now;
    }

    // Instance constructor (called when an instance of Configuration is created)
    public Configuration()
    {
        InstanceId = Guid.NewGuid().ToString(); // Generate a unique instance ID
    }
}


}
