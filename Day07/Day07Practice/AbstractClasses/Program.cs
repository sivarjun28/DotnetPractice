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

}

