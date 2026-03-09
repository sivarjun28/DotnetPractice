using System;
using System.Collections.Generic;

namespace Exercise04
{
/*
Components:
1. IFactory<T> interface
2. SimpleFactory<T> - creates new instances
3. PooledFactory<T> - reuses instances
4. CachedFactory<T> - caches by key

All factories must work with types that have parameterless constructors.
*/


    public interface IFactory<T> where T : new()
    {
        T Create();
        void Release(T instance);  // Return to pool if applicable
    }

    // SimpleFactory creates a new instance each time
    public class SimpleFactory<T> : IFactory<T> where T : new()
    {
        public T Create()
        {
            return new T();  // Simply creates a new instance
        }

        public void Release(T instance)
        {
            // Do nothing - no pooling
        }
    }

    // PooledFactory reuses instances from a pool
    public class PooledFactory<T> : IFactory<T> where T : new()
    {
        private readonly Stack<T> pool = new();
        private readonly int maxPoolSize;

        public int PoolSize => pool.Count;  // Get current size of the pool

        public PooledFactory(int maxPoolSize = 10)
        {
            this.maxPoolSize = maxPoolSize;
        }

        public T Create()
        {
            if (pool.Count > 0)
            {
                Console.WriteLine("Reusing from pool");
                return pool.Pop();  // Reuse an item from the pool
            }

            Console.WriteLine("Creating new instance");
            return new T();  // Create a new instance if pool is empty
        }

        public void Release(T instance)
        {
            if (pool.Count < maxPoolSize)
            {
                pool.Push(instance);  // Return the instance to the pool
                Console.WriteLine($"Returned to pool (size: {pool.Count})");
            }
            else
            {
                Console.WriteLine("Pool is full, cannot return instance");
            }
        }
    }

    // CachedFactory caches instances per key (Singleton pattern per key)
    public class CachedFactory<TKey, TValue> where TValue : new()
    {
        private readonly Dictionary<TKey, TValue> cache = new();

        public TValue GetOrCreate(TKey key)
        {
            if (!cache.ContainsKey(key))
            {
                cache[key] = new TValue();  // Create a new instance if not found
                Console.WriteLine($"Created new instance for key: {key}");
            }
            else
            {
                Console.WriteLine($"Reusing cached instance for key: {key}");
            }

            return cache[key];  // Return the instance
        }

        public void Clear()
        {
            cache.Clear();  // Clear the cache if needed
            Console.WriteLine("Cache cleared");
        }
    }

    // Test class - Connection
    public class Connection
    {
        public int ConnectionId { get; set; }
        public bool IsOpen { get; set; }

        public Connection()
        {
            ConnectionId = Random.Shared.Next(1000);  // Generate a random Connection ID
            IsOpen = true;  // Connections are open by default
        }
    }

    // Test scenario
    public class Program
    {
        public static void Main()
        {
            // Test SimpleFactory
            Console.WriteLine("Testing SimpleFactory:");
            IFactory<Connection> simpleFactory = new SimpleFactory<Connection>();
            var conn1 = simpleFactory.Create();  // Creates new instance
            var conn2 = simpleFactory.Create();  // Creates another new instance
            simpleFactory.Release(conn1);  // Does nothing in SimpleFactory
            simpleFactory.Release(conn2);  // Does nothing in SimpleFactory

            // Test PooledFactory
            Console.WriteLine("\nTesting PooledFactory:");
            IFactory<Connection> pooledFactory = new PooledFactory<Connection>(5);

            // Creating connections
            var conn3 = pooledFactory.Create();  // Creates new
            var conn4 = pooledFactory.Create();  // Creates new

            pooledFactory.Release(conn3);  // Returns conn3 to pool
            pooledFactory.Release(conn4);  // Returns conn4 to pool

            // Reusing connections from pool
            var conn5 = pooledFactory.Create();  // Reuses conn3
            var conn6 = pooledFactory.Create();  // Reuses conn4

            Console.WriteLine($"Pool Size: {((PooledFactory<Connection>)pooledFactory).PoolSize}");

            // Test CachedFactory
            Console.WriteLine("\nTesting CachedFactory:");
            var cachedFactory = new CachedFactory<string, Connection>();

            // Creating new connections with keys
            var conn7 = cachedFactory.GetOrCreate("db1");  // Creates new for "db1"
            var conn8 = cachedFactory.GetOrCreate("db2");  // Creates new for "db2"

            // Reusing cached connections
            var conn9 = cachedFactory.GetOrCreate("db1");  // Reuses cached connection for "db1"

            // Clear cache
            cachedFactory.Clear();
        }
    }
}