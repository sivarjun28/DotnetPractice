using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise05
{
    public class NamedCollection<T> : IEnumerable<T>, IEnumerable
    {
        private List<T> items = new();
        private Dictionary<string, T> namedItems = new();

        public int Count => items.Count;

        // Indexer By Integer
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return items[index];
            }
            set
            {
                if (index < 0 || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                items[index] = value;
            }
        }

        // Indexer By String (Key)
        public T this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key) || !namedItems.ContainsKey(key))
                    throw new ArgumentException($"The key {key} was not found");
                return namedItems[key];
            }
            set
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Key cannot be null or white space", nameof(key));
                }
                if (!namedItems.ContainsKey(key))
                {
                    items.Add(value);  // Optionally add to the list if the key is new
                }
                namedItems[key] = value;
            }
        }

        // Add method to add items to the collection
        public void Add(T item, string? name = null)
        {
            items.Add(item);
            if (name != null)
            {
                namedItems[name] = item;
            }
        }

        // Remove item by object
        public void Remove(T item)
        {
            // Find the key associated with the item in namedItems and remove
            foreach (var kvp in namedItems)
            {
                if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
                {
                    namedItems.Remove(kvp.Key);
                    break;
                }
            }

            // Also remove from the list
            items.Remove(item);
        }

        // Remove item by name
        public void Remove(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            if (namedItems.ContainsKey(name))
            {
                var itemToRemove = namedItems[name];
                items.Remove(itemToRemove);  // Remove from list
                namedItems.Remove(name);     // Remove from dictionary
            }
        }

        // Check if item exists by name
        public bool Contains(string name)
        {
            return namedItems.ContainsKey(name);
        }

        // Return all items
        public IEnumerable<T> GetAll()
        {
            return items;
        }

        // Enumerator for IEnumerable<T> (generic)
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        // Non-generic GetEnumerator (required for compatibility with older APIs)
        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator(); // This calls the generic version and returns IEnumerator
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            NamedCollection<string> collection = new();

            // Add items to the collection
            collection.Add("first item", "first");
            collection.Add("Second item", "second");
            collection.Add("Third item");

            // Access by index
            Console.WriteLine($"Item at index 0: {collection[0]}");

            // Access by name
            Console.WriteLine($"Item named 'first': {collection["first"]}");

            // Iterate through the collection
            Console.WriteLine("\nAll items:");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }

            // Removing an item by value
            collection.Remove("first item");

            Console.WriteLine("\nAll items after removal:");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }

            // Removing an item by name
            collection.Remove("second");

            Console.WriteLine("\nAll items after second removal:");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}