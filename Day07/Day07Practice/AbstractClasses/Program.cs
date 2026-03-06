using System;
namespace AbstractClasses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Usage
            // Animal animal = new Animal();  // ❌ Cannot instantiate abstract class
            Animal dog = new Dog { Name = "Buddy", Age = 3 };
            dog.MakeSound();  // Woof!
            Console.WriteLine(dog.Species);

            DataProcessor processor = new CsvDataProcessor();
            processor.ProcessData();

            IPlayable playable = new Song("nothing see", "Justin beiber", 5);
            playable.Play();
            playable.Pause();
            playable.Stop();

            List<IDrawable> drawables = new()
        {
            new Rectangle { Width = 100, Height = 50, X = 10, Y = 20 },
            new Circle { Radius = 25, X = 50, Y = 50 }
        };

            // Drawing all drawable objects
            foreach (IDrawable item in drawables)
            {
                item.Draw();  // Polymorphic call
            }

            // Demonstrating the use of IResizable and IRotatable interfaces on a Rectangle object
            var rectangle = new Rectangle { Width = 100, Height = 50, X = 10, Y = 20 };
            rectangle.Resize(1.5); // Resize the rectangle
            rectangle.Rotate(45);  // Rotate the rectangle

            // Demonstrating that a Circle does not implement IResizable or IRotatable
            var circle = new Circle { Radius = 25, X = 50, Y = 50 };
            // circle.Resize(1.5);   // Error: 'Circle' does not implement 'IResizable'
            // circle.Rotate(45);    // Error: 'Circle' does not implement 'IRotatable'

            ConsoleLogger logger = new();
            logger.Log("Info message");

            RoboDog roboDog = new();
            roboDog.Bark();

            IAnimal animal = roboDog;
            animal.MakeSound();

            IRobot robot = roboDog;
            robot.MakeSound();

            DataService dataService = new DataService(new SqlRepository());
            DataService dataService1 = new DataService(new FileRepository());

            dataService.ProcessData("Arjun");
            new SqlRepository().Save("Siva");
            new SqlRepository().Load(1);

            new FileRepository().Save("Siva");
            new FileRepository().Load(1);

            ShoppingCart cart = new();
            cart.SetPaymentStrategy(new CreditCardPayment());
            cart.Checkout(78.99m);
            cart.SetPaymentStrategy(new PayPalPayment());
            cart.Checkout(49.99m);
            


        }
    }

    // ABSTRACT CLASS - cannot be instantiated
    public abstract class Animal
    {
        // Regular property
        public string Name { get; set; }
        public int Age { get; set; }

        // Abstract property - must be implemented
        public abstract string Species { get; }

        // Regular method with implementation
        public void Eat()
        {
            Console.WriteLine($"{Name} is eating");
        }

        // Abstract method - no implementation
        public abstract void MakeSound();

        // Virtual method - can be overridden
        public virtual void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping");
        }
    }

    // Concrete implementation
    public class Dog : Animal
    {
        // Must implement abstract property
        public override string Species => "Canis familiaris";

        // Must implement abstract method
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof!");
        }

        // Can optionally override virtual method
        public override void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping in the kennel");
        }
    }

    public abstract class DataProcessor
    {
        // Template method - defines algorithm structure
        public void ProcessData()
        {
            LoadData();
            ValidateData();
            TransformData();
            SaveData();
            Cleanup();
        }

        // Abstract methods - must implement
        protected abstract void LoadData();
        protected abstract void TransformData();
        protected abstract void SaveData();

        // Virtual methods - optional to override
        protected virtual void ValidateData()
        {
            Console.WriteLine("Performing basic validation");
        }

        protected virtual void Cleanup()
        {
            Console.WriteLine("Performing basic cleanup");
        }
    }

    public class CsvDataProcessor : DataProcessor
    {
        protected override void LoadData()
        {
            Console.WriteLine("Loading data from CSV file");
        }

        protected override void TransformData()
        {
            Console.WriteLine("Transforming CSV data");
        }

        protected override void SaveData()
        {
            Console.WriteLine("Saving to database");
        }

        // Override optional method
        protected override void ValidateData()
        {
            base.ValidateData();  // Call base implementation
            Console.WriteLine("Performing CSV-specific validation");
        }
    }
    // INTERFACE - contract definition
    public interface IPlayable
    {
        // Properties (no implementation)
        string Name { get; set; }
        int Duration { get; }

        // Methods (no implementation)
        void Play();
        void Pause();
        void Stop();


    }

    // Implementation
    public class Song : IPlayable
    {
        public string Name { get; set; } = string.Empty;
        public int Duration { get; private set; }
        public string Artist { get; set; } = string.Empty;

        public Song(string name, string artist, int duration)
        {
            Name = name;
            Artist = artist;
            Duration = duration;
        }

        public void Play()
        {
            Console.WriteLine($"Playing: {Name} by {Artist}");
        }

        public void Pause()
        {
            Console.WriteLine($"Paused: {Name}");
        }

        public void Stop()
        {
            Console.WriteLine($"Stopped: {Name}");
        }
    }
    public interface IDrawable
    {
        void Draw();
    }

    public interface IResizable
    {
        void Resize(double factor);
    }

    public interface IRotatable
    {
        void Rotate(double degrees);
    }

    // Class implementing multiple interfaces
    public class Rectangle : IDrawable, IResizable, IRotatable
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public void Draw()
        {
            Console.WriteLine($"Drawing rectangle at ({X}, {Y}) - {Width}x{Height}");
        }

        public void Resize(double factor)
        {
            Width *= factor;
            Height *= factor;
            Console.WriteLine($"Resized to {Width}x{Height}");
        }

        public void Rotate(double degrees)
        {
            Console.WriteLine($"Rotated {degrees} degrees");
        }
    }

    // Circle class implementing the IDrawable interface
    public class Circle : IDrawable
    {
        public double Radius { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public void Draw()
        {
            Console.WriteLine($"Drawing circle at ({X}, {Y}) - Radius: {Radius}");
        }
    }

    //Default Interface Methods
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message)
        {
            Log($"Error: {message}");
        }

        void LogWarning(string message)
        {
            Log($"WARNING: {message}");
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }


    }

    // USE ABSTRACT CLASS when:
    // - You need to share implementation
    // - You need protected members
    // - You have a clear "is-a" relationship

    public abstract class Vehicle
    {
        protected int wheels;
        protected string fuelType;

        // Shared implementation
        public void StartEngine()
        {
            Console.WriteLine("Engine started");
        }

        // Force implementation
        public abstract void Move();
    }

    // USE INTERFACE when:
    // - You want to define capabilities
    // - You need multiple inheritance
    // - You want to ensure specific behavior

    public interface IFlyable
    {
        double MaxAltitude { get; }
        void TakeOff();
        void Land();
    }

    public interface ISwimmable
    {
        double MaxDepth { get; }
        void Dive();
        void Surface();
    }

    // Can implement multiple interfaces but inherit from one class
    public class Duck : IFlyable, ISwimmable
    {
        public double MaxAltitude => 1000;
        public double MaxDepth => 10;


        public void TakeOff()
        {
            Console.WriteLine("Duck taking off");
        }

        public void Land()
        {
            Console.WriteLine("Duck landing");
        }

        public void Dive()
        {
            Console.WriteLine("Duck diving");
        }

        public void Surface()
        {
            Console.WriteLine("Duck surfacing");
        }
    }

    //Advanced Interface Concepts 
    public interface IAnimal
    {
        void MakeSound();

    }

    public interface IRobot
    {
        void MakeSound();
    }
    public class RoboDog : IAnimal, IRobot
    {
        // Explicit implementation for IAnimal
        void IAnimal.MakeSound()
        {
            System.Console.WriteLine("Woof (organic)"); ;
        }
        void IRobot.MakeSound()
        {
            System.Console.WriteLine("Beep boop (robotic)"); ;
        }
        public void Bark()
        {
            System.Console.WriteLine("Regular Bark");
        }
    }

    // ❌ BAD: Fat interface
    public interface IWorker
    {
        void Work();
        void Eat();
        void Sleep();
        void GetPaid();
    }

    // ✅ GOOD: Segregated interfaces
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
        void GetPaid();
    }

    // Human implements all
    public class Human : IWorkable, IFeedable, ISleepable, IPayable
    {
        public void Work() { }
        public void Eat() { }
        public void Sleep() { }
        public void GetPaid() { }
    }

    // Robot only implements what makes sense
    public class Robot : IWorkable
    {
        public void Work() { }
        // Doesn't need Eat, Sleep, GetPaid
    }

    //Dependency Inversion with interfaces
    public interface IDataRepository
    {
        void Save(string data);
        string Load(int id);


    }

    public class SqlRepository : IDataRepository
    {
        public void Save(string data)
        {
            System.Console.WriteLine($"Saving to sQl: {data}");
        }

        public string Load(int id)
        {
            return $"Data from sql: {id}";
        }
    }

    public class FileRepository : IDataRepository
{
    public void Save(string data)
    {
        Console.WriteLine($"Saving to file: {data}");
    }
    
    public string Load(int id)
    {
        return $"Data from file: {id}";
    }
}

    public class DataService
    {
        private readonly IDataRepository dataRepository;
        public DataService(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public void ProcessData(string data)
        {
            System.Console.WriteLine(data);
        }
    }

    // Common Patterns
    public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

// public class ProductRepository : IRepository<Product>
// {
//     private List<Product> products = new();
    
//     public Product GetById(int id) => products.FirstOrDefault(p => p.Id == id);
//     public IEnumerable<Product> GetAll() => products;
//     public void Add(Product entity) => products.Add(entity);
//     public void Update(Product entity) { /* implementation */ }
//     public void Delete(int id) => products.RemoveAll(p => p.Id == id);
// }

public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} with credit card");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} with PayPal");
    }
}

public class ShoppingCart
{
    private IPaymentStrategy paymentStrategy;
    
    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }
    
    public void Checkout(decimal amount)
    {
        paymentStrategy.Pay(amount);
    }
}
 
}

