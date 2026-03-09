using System;
using System.Collections.Generic;

namespace Exercise01
{
    public class CircularBuffer<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int count;

        public int Capacity { get; }
        public int Count => count;
        public bool IsFull => count == Capacity;
        public bool IsEmpty => count == 0;

        public CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be positive.");
            Capacity = capacity;
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }

        // Add item to buffer, overwrite the oldest if full
        public void Add(T item)
        {
            if (IsFull)
            {
                head = (head + 1) % Capacity; // Overwrite oldest item if buffer is full
            }
            else
            {
                count++;
            }
            buffer[tail] = item;
            tail = (tail + 1) % Capacity; // Move the tail forward
        }

        // Get the oldest item without removing it
        public T GetOldest()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty.");
            return buffer[head];
        }

        // Remove and return the oldest item
        public T Remove()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty.");
            T oldest = buffer[head];
            buffer[head] = default; // Optionally clear the reference
            head = (head + 1) % Capacity; // Move the head forward
            count--;
            return oldest;
        }

        // Clear the buffer
        public void Clear()
        {
            buffer = new T[Capacity];
            head = 0;
            tail = 0;
            count = 0;
        }

        // Get all items from oldest to newest
        public IEnumerable<T> GetAll()
        {
            if (IsEmpty) yield break;

            int current = head;
            for (int i = 0; i < count; i++)
            {
                yield return buffer[current];
                current = (current + 1) % Capacity;
            }
        }
    }

    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> queue = new List<T>();

        public int Count => queue.Count;

        // Enqueue an item with priority (highest first)
        public void Enqueue(T item)
        {
            queue.Add(item);
            int childIndex = queue.Count - 1;
            int parentIndex = (childIndex - 1) / 2;

            // Bubble up to maintain heap property (highest priority at root)
            while (childIndex > 0 && queue[childIndex].CompareTo(queue[parentIndex]) > 0)
            {
                T temp = queue[childIndex];
                queue[childIndex] = queue[parentIndex];
                queue[parentIndex] = temp;
                childIndex = parentIndex;
                parentIndex = (childIndex - 1) / 2;
            }
        }

        // Dequeue the item with the highest priority
        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Queue is empty.");

            T result = queue[0];

            // Move the last item to the root and bubble down to maintain heap property
            queue[0] = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);

            int parentIndex = 0;
            int leftChildIndex = 1;
            int rightChildIndex = 2;

            while (leftChildIndex < queue.Count)
            {
                int largest = parentIndex;

                if (queue[leftChildIndex].CompareTo(queue[largest]) > 0)
                    largest = leftChildIndex;
                if (rightChildIndex < queue.Count && queue[rightChildIndex].CompareTo(queue[largest]) > 0)
                    largest = rightChildIndex;

                if (largest == parentIndex)
                    break;

                // Swap
                T temp = queue[parentIndex];
                queue[parentIndex] = queue[largest];
                queue[largest] = temp;

                parentIndex = largest;
                leftChildIndex = 2 * parentIndex + 1;
                rightChildIndex = 2 * parentIndex + 2;
            }

            return result;
        }

        // Peek the item with the highest priority without removing it
        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Queue is empty.");
            return queue[0];
        }
    }

    public class BoundedStack<T>
    {
        private T[] stack;
        private int top;

        public int Capacity { get; }
        public int Count => top;
        public bool IsFull => top == Capacity;

        public BoundedStack(int capacity)
        {
            if (capacity <= 0)
                throw new InvalidOperationException("Capacity must be positive.");
            Capacity = capacity;
            stack = new T[capacity];
            top = 0;
        }

        // Push an item onto the stack
        public void Push(T item)
        {
            if (IsFull)
                throw new InvalidOperationException("Stack is full.");
            stack[top++] = item;
        }

        // Pop the top item from the stack
        public T Pop()
        {
            if (top == 0)
                throw new InvalidOperationException("Stack is empty.");
            T item = stack[--top];
            stack[top] = default; // Clear the reference
            return item;
        }

        // Peek the top item without removing it
        public T Peek()
        {
            if (top == 0)
                throw new InvalidOperationException("Stack is empty.");
            return stack[top - 1];
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Test CircularBuffer
            CircularBuffer<int> circular = new CircularBuffer<int>(3);
            circular.Add(1);
            circular.Add(2);
            circular.Add(3);
            circular.Add(9);  // Overwrites the oldest value (1)
            circular.Add(4);  // Overwrites the next oldest value (2)

            // Print all elements in the buffer
            foreach (int item in circular.GetAll())
            {
                Console.WriteLine(item);  // Expected: 3, 9, 4
            }

            // Get and print the oldest item
            int oldest = circular.GetOldest();
            Console.WriteLine($"Oldest: {oldest}");  // Expected: 3

            // Remove and print the oldest item
            int removed = circular.Remove();
            Console.WriteLine($"Removed: {removed}");  // Expected: 3

            // Clear the buffer
            circular.Clear();
            Console.WriteLine($"Buffer count after clear: {circular.Count}");  // Expected: 0

            PriorityQueue<int> pq = new PriorityQueue<int>();

            pq.Enqueue(5);
            pq.Enqueue(10);
            pq.Enqueue(3);
            pq.Enqueue(8);

            // Print the queue after enqueueing the values
            Console.WriteLine("Queue after enqueuing:");
            while (pq.Count > 0)
            {
                Console.WriteLine(pq.Dequeue());  // Dequeue by priority (highest first)
            }

            // Peek after adding items
            pq.Enqueue(12);
            pq.Enqueue(6);
            Console.WriteLine($"Peek: {pq.Peek()}");  // Peek the highest priority item

            BoundedStack<int> bs = new BoundedStack<int>(5);

            bs.Push(5);
            bs.Push(10);
            bs.Push(3);
            bs.Push(8);

            // Print the stack after pushing values
            Console.WriteLine("Stack after pushing:");
            while (bs.Count > 0)
            {
                Console.WriteLine(bs.Pop());  // Pop the top item
            }

            // Peek after pushing items
            bs.Push(12);
            bs.Push(6);
            Console.WriteLine($"Peek: {bs.Peek()}");  // Peek the top item
        }
    }
}