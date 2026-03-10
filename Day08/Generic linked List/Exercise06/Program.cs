using System;
using System.Collections;
using System.Collections.Generic;
namespace Exercise06
{


    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T>? Next { get; set; }
        public Node<T>? Previous { get; set; }

        public Node(T value)
        {
            Value = value;
        }
    }

    public class LinkedList<T> : ICollection<T>
    {
        private Node<T>? _first;
        private Node<T>? _last;
        private int _count;

        public LinkedList()
        {
            _first = _last = null;
            _count = 0;
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public Node<T>? First => _first;

        public Node<T>? Last => _last;

        // Add a new node at the beginning
        public void AddFirst(T value)
        {
            var newNode = new Node<T>(value);
            if (_first == null)
            {
                _first = _last = newNode;
            }
            else
            {
                newNode.Next = _first;
                _first.Previous = newNode;
                _first = newNode;
            }
            _count++;
        }

        // Add a new node at the end
        public void AddLast(T value)
        {
            var newNode = new Node<T>(value);
            if (_last == null)
            {
                _first = _last = newNode;
            }
            else
            {
                newNode.Previous = _last;
                _last.Next = newNode;
                _last = newNode;
            }
            _count++;
        }

        // Add a node before another node
        public void AddBefore(Node<T> node, T value)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            var newNode = new Node<T>(value);
            newNode.Next = node;
            newNode.Previous = node.Previous;

            if (node.Previous != null)
            {
                node.Previous.Next = newNode;
            }
            else
            {
                _first = newNode;
            }

            node.Previous = newNode;
            _count++;
        }

        // Add a node after another node
        public void AddAfter(Node<T> node, T value)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            var newNode = new Node<T>(value);
            newNode.Previous = node;
            newNode.Next = node.Next;

            if (node.Next != null)
            {
                node.Next.Previous = newNode;
            }
            else
            {
                _last = newNode;
            }

            node.Next = newNode;
            _count++;
        }

        // Remove the first node
        public void RemoveFirst()
        {
            if (_first == null) throw new InvalidOperationException("The list is empty.");
            if (_first.Next != null)
            {
                _first = _first.Next;
                _first.Previous = null;
            }
            else
            {
                _first = _last = null;
            }
            _count--;
        }

        // Remove the last node
        public void RemoveLast()
        {
            if (_last == null) throw new InvalidOperationException("The list is empty.");
            if (_last.Previous != null)
            {
                _last = _last.Previous;
                _last.Next = null;
            }
            else
            {
                _first = _last = null;
            }
            _count--;
        }

        // Remove a node by value
        public bool Remove(T value)
        {
            var node = Find(value);
            if (node == null)
                return false;

            if (node.Previous != null)
                node.Previous.Next = node.Next;
            else
                _first = node.Next;

            if (node.Next != null)
                node.Next.Previous = node.Previous;
            else
                _last = node.Previous;

            _count--;
            return true;
        }

        // Find a node by value
        public Node<T>? Find(T value)
        {
            var current = _first;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, value))
                    return current;
                current = current.Next;
            }
            return null;
        }

        // Check if a value exists in the list
        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        // Clear the list
        public void Clear()
        {
            _first = _last = null;
            _count = 0;
        }

        // Enumerator for foreach support
        public IEnumerator<T> GetEnumerator()
        {
            var current = _first;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        // Explicit implementation of IEnumerable for non-generic use
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Add a value to the list (from ICollection<T> interface)
        public void Add(T item)
        {
            AddLast(item);
        }

        // Copy elements to an array (from ICollection<T> interface)
        public void CopyTo(T[] array, int arrayIndex)
        {
            var current = _first;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

      
    }

    class Program
    {
        static void Main()
        {
            LinkedList<int> list = new();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);
            list.AddFirst(0);

            // Iterate through the list
            foreach (int value in list)
            {
                Console.WriteLine(value);  // Output: 0, 1, 2, 3
            }

            // Remove a value and check if it exists
            list.Remove(2);
            bool contains = list.Contains(2);  // false
            Console.WriteLine(contains);

            // Find a node and add a value after it
            Node<int>? node = list.Find(1);
            if (node != null)
            {
                list.AddAfter(node, 99);  // 0, 1, 99, 3
            }

            // Output updated list
            foreach (int value in list)
            {
                Console.WriteLine(value);  // Output: 0, 1, 99, 3
            }
        }
    }
}