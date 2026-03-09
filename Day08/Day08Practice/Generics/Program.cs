using System;
using System.Text.RegularExpressions;
namespace Generics
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ObjectStack stack = new();
            stack.Push(42);
            int value = (int)stack.Pop();
            System.Console.WriteLine(value);

            Stack<int> intStack = new();
            intStack.Push(23);
            intStack.Push(32);
            intStack.Pop();
            int value1 = intStack.Peek();
            System.Console.WriteLine(value1);

            Stack<string> stack1 = new();
            stack1.Push("Arjun");
            stack1.Push("Shiva");
            stack1.Pop();
            string value2 = stack1.Peek();
            System.Console.WriteLine(value2);


            int x = 10, y = 20;
            Utilities.Swap(ref x, ref y);
            System.Console.WriteLine($"X : {x} , Y: {y}");

            var pair = Utilities.CreatePair("name", 42);

            List<int> numbers = new() { 1, 2, 3, 4, 5, 6 };
            int? result = Utilities.Find(numbers, n => n > 3);
            System.Console.WriteLine(result);

            List<string> names = new() { "Alice", "Arjun", "Shiva" };
            if (!names.IsEmpty())
            {
                string randomName = names.GetRandomElement();
                System.Console.WriteLine($"Random name: {randomName}");
            }

            List<int> numbers1 = new() { 1, 2, 3, 4, 5 };
            List<int> shuffled = numbers.Shuffle();
            foreach (var items in shuffled)
            {
                System.Console.WriteLine(items);
            }
            Factory<Product> factory = new();
            Product product = factory.Create();
            System.Console.WriteLine(product);

            AdvanceRepository<Product> repo = new();
            Product product1 = repo.Create(1);
            System.Console.WriteLine(product);


            // LIST<T> - Dynamic array
            List<int> numbers2 = new() { 1, 2, 3, 4, 5 };
            numbers2.Add(6);
            numbers2.Remove(3);
            int first = numbers2[0];

            // DICTIONARY<TKey, TValue> - Key-value pairs
            Dictionary<string, int> ages = new()
            {
                ["Alice"] = 30,
                ["Bob"] = 25,
                ["Charlie"] = 35
            };

            ages.Add("David", 28);
            int aliceAge = ages["Alice"];
            bool hasKey = ages.ContainsKey("Bob");
            ages.Remove("Charlie");

            // HASHSET<T> - Unique values
            HashSet<string> uniqueNames = new() { "Alice", "Bob", "Charlie" };
            uniqueNames.Add("Alice");  // Won't add duplicate
            bool added = uniqueNames.Add("David");  // true
            bool contains = uniqueNames.Contains("Bob");  // true

            // QUEUE<T> - FIFO (First-In-First-Out)
            Queue<string> queue = new();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");
            string first1 = queue.Dequeue();  // "First"
            string peek = queue.Peek();      // "Second" (doesn't remove)

            // STACK<T> - LIFO (Last-In-First-Out)
            Stack<int> stack2 = new();
            stack2.Push(1);
            stack2.Push(2);
            stack2.Push(3);
            int top = stack2.Pop();   // 3
            int peek2 = stack2.Peek(); // 2

            // LINKEDLIST<T> - Doubly-linked list
            LinkedList<string> linkedList = new();
            linkedList.AddFirst("First");
            linkedList.AddLast("Last");
            linkedList.AddAfter(linkedList.First!, "Second");

            // SORTEDSET<T> - Sorted unique values
            SortedSet<int> sortedSet = new() { 5, 2, 8, 1, 9 };
            // Automatically sorted: { 1, 2, 5, 8, 9 }

            // Usage
            IRepository<Product> productRepo = new InMemoryRepository<Product>();
            productRepo.Add(new Product { Id = 1, Name = "Laptop" });
            Product? product3 = productRepo.GetById(1);
            if (product3 != null)
            {
                System.Console.WriteLine(product3);  // Print product details
            }
            else
            {
                System.Console.WriteLine("Product not found.");
            }

            // Usage
            List<Person> people = new()
{
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

            people.Sort();  // Uses IComparable<Person>
                            // Now sorted by age: Bob(25), Alice(30), Charlie(35)

            foreach (var item in people)
            {
                System.Console.WriteLine(item);
            }

            Person alice1 = new Person { Name = "Alice", Age = 30 };
            Person alice2 = new Person { Name = "Alice", Age = 30 };
            bool equal = alice1.Equals(alice2);  // true (uses IEquatable<Person>)
            System.Console.WriteLine(equal);

        }
    }

    //without genrics
    public class IntStack
    {
        private List<int> items = new();
        public void Push(int item) => items.Add(item);
        public int Pop() => items[^1];
    }

    public class StringStack
    {
        private List<string> items = new();
        public void Push(string item) => items.Add(item);
        public string Pop() => items[^1];
    }

    public class ObjectStack
    {
        private List<object> items = new();
        public void Push(object item) => items.Add(item);
        public object Pop() => items[^1];
    }

    //With Generics
    public class Stack<T>
    {
        public List<T> items = new();
        public void Push(T item)
        {
            items.Add(item);
        }

        public T Pop()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Stack is Empty");
            }
            T item = items[^1];
            items.RemoveAt(items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            T item = items[^1];
            return item;
        }
        public int Count => items.Count;
    }
    // Generic Methods 

    public class Utilities
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Generic method with multiple type parameters
        public static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        //Generic MNethod To find Items

        public static T? Find<T>(List<T> list, Predicate<T> match)
        {
            foreach (T item in list)
            {
                if (match(item))
                    return item;
            }
            return default(T);  // null for reference types, 0 for value types
        }

    }

    //Generic Extension Methods
    public static class Extensions
    {
        // Generic extension method
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            Random random = new();
            int index = random.Next(list.Count);
            return list[index];
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random random = new();
            return list.OrderBy(x => random.Next()).ToList();
        }
    }

    // Generic Constraints
    // 1. CLASS CONSTRAINT - must be reference type
    public class Repository<T> where T : class
    {
        private List<T> items = new();

        public void Add(T item)
        {
            if (item == null)  // Can check for null with class constraint
                throw new ArgumentNullException(nameof(item));

            items.Add(item);
        }
    }


    // 2. STRUCT CONSTRAINT - must be value type
    public class ValueStorage<T> where T : struct
    {
        private T value;

        public bool HasValue { get; private set; }

        public T Value
        {
            get => HasValue ? value : throw new InvalidOperationException();
            set
            {
                this.value = value;
                HasValue = true;
            }
        }
    }

    public class Factory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }

        public List<T> CreateMany(int count)
        {
            List<T> items = new();
            foreach (var item in items)
            {
                items.Add(item);
            }
            return items;
        }
    }

    // 5. INTERFACE CONSTRAINT
    public class Sorter<T> where T : IComparable<T>
    {
        public List<T> Sort(List<T> items)
        {
            return items.OrderBy(x => x).ToList();
        }
    }
    //6.Multiple Constaints
    public class AdvanceRepository<T> where T : class, IEntity, new()
    {
        // T must be:
        // - Reference type (class)
        // - Implement IEntity
        // - Have parameterless constructor (new())

        public T Create(int id)
        {
            T item = new T();
            item.Id = id;
            return item;
        }
    }

    public interface IEntity
    {
        int Id { get; set; }
        string Name { get; set; }

    }

    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Product()
        {

        }

    }

    //Generic Interfaces
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        T? GetById(int id);
        IEnumerable<T> GetAll();
    }

    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private List<T> items = new();
        public void Add(T item)
        {
            items.Add(item);
        }
        public void Remove(T item)
        {
            items.Remove(item);
        }
        public T? GetById(int id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return items;
        }
    }

    //IComparable and IEquatable
    public class Person : IComparable<Person>, IEquatable<Person>
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        // IComparable<T> - for sorting
        public int CompareTo(Person? other)
        {
            if (other == null) return 1;
            return Age.CompareTo(other.Age);
        }
        public bool Equals(Person? other)
        {
            if (other == null) return false;

            return Name == other.Name && Age == other.Age;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Person);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Age);
        }

        public override string ToString()
        {
            return $"{Name}({Age})";
        }
    }

}
