using System;
using System.Collections;
namespace AdvancedGenerics
{
    // Generic Delegates 
    internal class Program
    {
        static Func<int, int, int> add = (a, b) => a + b;

        static Func<string, int> getLength = s => s.Length;

        static Func<int, bool> isEven = e => e % 2 == 0;

        // ACTION<T> - Returns void
        static Action<string> print = message => System.Console.WriteLine(message);

        static Action<int, int> printSum = (a, b) => System.Console.WriteLine($"sum of {a}, {b} is {a + b}");


        //predicate Return bool
        static Predicate<int> isPositive = n => n > 0;
        static Predicate<string> isEmpty = n => string.IsNullOrEmpty(n);
        static void Main(string[] args)
        {
            int sum = add(5, 3);
            System.Console.WriteLine(sum);
            int length = getLength("Arjun");
            System.Console.WriteLine(length);
            bool even = isEven(56);
            System.Console.WriteLine(even);
            print("I am working at VSC");
            printSum(56, 4);
            bool positive = isPositive(9);
            System.Console.WriteLine(positive);
            bool empty = isEmpty("");
            System.Console.WriteLine(empty);

            DataProcessor<int> dataProcessor = new();
            dataProcessor.AddItem(1);
            dataProcessor.AddItem(89);
            dataProcessor.AddItem(8);
            dataProcessor.AddItem(98);

            List<int> evens = dataProcessor.Filter(n => n % 2 == 0);
            foreach (var item in evens)
            {
                System.Console.Write(item + ", ");
            }

            List<string> strings = dataProcessor.Transform(n => $"Number: {n}");
            foreach (var item in strings)
            {
                System.Console.Write(item);
            }

            dataProcessor.ForEach(n => System.Console.WriteLine("\n" + n));
            // Covariance in action
            IProducer<Dog> dogProducer = new DogProducer();

            // ✅ Can assign to less derived type (Dog -> Animal)
            IProducer<Animal> animalProducer = dogProducer;  // Covariance

            // Produce an Animal (but we know it's a Dog)
            Animal animal = animalProducer.Produce();  // Works! Gets Dog

            // Even though `animal` is of type `Animal`, we know it's actually a `Dog`
            Console.WriteLine(animal.Name);  // Outputs "Dog"
            animal.Speak();  // Outputs "Animal speaks!" (inherited method)

            // Casting back to Dog to call Dog-specific methods
            if (animal is Dog dog)
            {
                dog.Bark();  // Outputs "Woof!" (Dog-specific method)
            }

            // IEnumerable<T> is covariant
            List<string> strings1 = new() { "Hello", "World" };

            // ✅ Can assign to IEnumerable<object>
            IEnumerable<object> objects = strings1;  // Covariance!

            foreach (object obj in objects)
            {
                Console.WriteLine(obj);  // Works - strings are objects
            }

            // Another example
            List<Dog> dogs = new() { new Dog(), new Dog() };
            IEnumerable<Animal> animals = dogs;  // Covariance

            // Contravariance in action
            IConsumer<Animal> animalConsumer = new AnimalConsumer();

            // ✅ Can assign to more derived type (Animal -> Dog)
            IConsumer<Dog> dogConsumer = animalConsumer;  // Contravariance!

            dogConsumer.Consume(new Dog());  // Works! AnimalConsumer can consume dogs
                                             // Usage
            List<int> numbers = Enumerable.Range(1, 100).ToList();
            PagedCollection<int> paged = new(numbers, 10);

            Console.WriteLine($"Total pages: {paged.TotalPages}");

            var page1 = paged.GetPage(1);  // 1-10
            var page2 = paged.GetPage(2);  // 11-20


            // Usage
            ObservableCollection<int> collection = new();

            collection.ItemAdded += item => Console.WriteLine($"Added: {item}");
            collection.ItemRemoved += item => Console.WriteLine($"Removed: {item}");
            collection.collectionCleared += () => Console.WriteLine("Cleared!");

            collection.Add(1);     // Triggers ItemAdded
            collection.Add(2);     // Triggers ItemAdded
            collection.Remove(1);  // Triggers ItemRemoved
            collection.Clear();
        }


    }

    //Using Delegates with Collections
    public class DataProcessor<T>
    {
        public List<T> items = new();
        public void AddItem(T item) => items.Add(item);

        // Use Func<T, bool> for filtering\
        public List<T> Filter(Func<T, bool> predicate)
        {
            List<T> result = new();
            foreach (T item in items)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // Use Func<T, TResult> for transformation
        public List<TResult> Transform<TResult>(Func<T, TResult> selector)
        {
            List<TResult> result = new();
            foreach (T item in items)
            {
                result.Add(selector(item));
            }
            return result;
        }
        // Use Action<T> for iteration
        public void ForEach(Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }
    }

    //Covariance with out keyword
    // COVARIANCE - Can use more derived type
    // Applies to: interfaces and delegates with 'out' keyword

    // Base class
    public class Animal
    {
        public string Name { get; set; } = "Generic Animal";

        public void Speak()
        {
            Console.WriteLine("Animal speaks!");
        }
    }

    // Derived class
    public class Dog : Animal
    {
        public Dog()
        {
            Name = "Dog";
        }

        public void Bark()
        {
            Console.WriteLine("Woof!");
        }
    }

    // Interface with covariance
    public interface IProducer<out T>
    {
        T Produce();  // Method to produce an instance of type T
    }

    // Implementation of the IProducer interface for Animal
    public class AnimalProducer : IProducer<Animal>
    {
        public Animal Produce() => new Animal();  // Produces an Animal
    }

    // Implementation of the IProducer interface for Dog
    public class DogProducer : IProducer<Dog>
    {
        public Dog Produce() => new Dog();  // Produces a Dog
    }

    // CONTRAVARIANCE - Can use less derived type
    // Applies to: interfaces and delegates with 'in' keyword

    public interface IConsumer<in T>  // 'in' = contravariant
    {
        void Consume(T item);  // Can only take T as parameter (input position)
                               // T Produce();  // ❌ Cannot return T
    }

    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal animal)
        {
            Console.WriteLine($"Consuming {animal.GetType().Name}");
        }
    }

    public class PagedCollection<T> : IEnumerable<T>
    {
        private readonly List<T> items;
        private readonly int pageSize;

        public int TotalPages => (int)Math.Ceiling((double)items.Count / pageSize);
        public int TotalItems => items.Count;

        public PagedCollection(IEnumerable<T> items, int pageSize)
        {
            this.items = items.ToList();
            this.pageSize = pageSize;
        }

        public IEnumerable<T> GetPage(int pageNumber)
        {
            return items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class ObservableCollection<T> : ICollection<T>
    {
        public readonly List<T> items = new();

        public event Action<T>? ItemAdded;
        public event Action<T>? ItemRemoved;
        public event Action collectionCleared;

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            items.Add(item);
            ItemAdded?.Invoke(item);
        }

        public void Clear()
        {
            items.Clear();
            collectionCleared?.Invoke();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {

            bool removed = items.Remove(item);
            if (removed)
            {
                ItemRemoved?.Invoke(item);
            }
            return removed;
        }

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

   public class QueryBuilder<T>
{
    private IEnumerable<T> source;
    private readonly List<Func<T, bool>> filters = new();
    private Func<T, object>? orderBy;
    private bool descending;
    private int? take;
    private int? skip;
    
    public QueryBuilder(IEnumerable<T> source)
    {
        this.source = source;
    }
    
    public QueryBuilder<T> Where(Func<T, bool> predicate)
    {
        filters.Add(predicate);
        return this;
    }
    
    public QueryBuilder<T> OrderBy(Func<T, object> selector)
    {
        orderBy = selector;
        descending = false;
        return this;
    }
    
    public QueryBuilder<T> OrderByDescending(Func<T, object> selector)
    {
        orderBy = selector;
        descending = true;
        return this;
    }
    
    public QueryBuilder<T> Take(int count)
    {
        take = count;
        return this;
    }
    
    public QueryBuilder<T> Skip(int count)
    {
        skip = count;
        return this;
    }
    
    public List<T> ToList()
    {
        IEnumerable<T> query = source;
        
        // Apply filters
        foreach (var filter in filters)
        {
            query = query.Where(filter);
        }
        
        // Apply ordering
        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }
        
        // Apply skip
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        
        // Apply take
        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }
        
        return query.ToList();
    }
}



}
