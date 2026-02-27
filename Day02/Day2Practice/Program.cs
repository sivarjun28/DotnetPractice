using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Linq; // Make sure to include this for ToArray()

namespace Day2Practice
{
    internal class Program
    {
        static string GetDayType(int day)
            {
                switch (day)
                {
                    case 1:
                    case 7:
                        return "Weekend";
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        return "Weekday";
                    default:
                        return "Invalid";
                }
            }
            
            // NEW WAY (C# 8+ switch expression)
            static string GetDayType1(int day) => day switch
            {
                1 or 7 => "Weekend",
                >= 2 and <= 6 => "Weekday",
                _ => "Invalid"
            };
            // Pattern matching
            static string DescribeNumber(object obj) => obj switch
            {
                int n when n < 0 => "Negative integer",
                int n when n == 0 => "Zero",
                int n => $"Positive integer: {n}",
                double d => $"Double: {d}",
                string s => $"String: {s}",
                _ => "Unknown type"
            };
        public static int Add(int a, int b)
        {
            return a + b;
        }

        // Expression-bodied method (C# 6+)
        public static int Add1(int a, int b) => a + b;

        // Optional parameters
        public static void Greet(string name, string greeting = "hello")
        {
            Console.WriteLine($"{greeting}, {name}");
        }

        // Method overloading
        public static int Calculate(int a, int b) => a * b;
        public static double Calculate(double a, double b) => a * b;
        public static int Calculate(int a, int b, int c) => a + b + c;

        // By VALUE (default) - Copy is passed
        static void IncreamentVal(int num)
        {
            num++;
        }

        // By REFERENCE (ref) - Original is modified
        static void IncreamentVal1(ref int num1)
        {
            num1++;
        }

        // TryParse example
        static bool TryParse(string input, out int result)
        {
            result = 0;
            return int.TryParse(input, out result);
        }

        // Modify value by value (doesn't affect the original variable)
        static void ModifyValue(int num)
        {
            num = 50;
        }

        // Modify value by reference (affects the original variable)
        static void ModifyValueRef(ref int num)
        {
            num = 50;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(GetDayType(2));
            

            Console.WriteLine(GetDayType1(8));

            
            Console.WriteLine(DescribeNumber(21));
            Console.WriteLine(DescribeNumber(22.9));
            Console.WriteLine(DescribeNumber(-32));

            // Loops - Key Differences
            string[] names = { "Arjun", "Sumanth", "Gopi" };
            // foreach loop
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }

            // for loop
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i} => {names[i]}");
            }

            // while loop
            int count = 0;
            while (count < 5)
            {
                Console.WriteLine(count);
                count++;
            }

            // Do-while loop
            do
            {
                Console.WriteLine("Executed once");
            } while (false);

            foreach (int i in Enumerable.Range(1, 10))
            {
                Console.WriteLine(i);
            }

            // Standard Method
            int res = Add(1, 2);
            Console.WriteLine(res);

            Greet(greeting: "Hi", name: "Alice");
            int res1 = Calculate(2, 3);
            double res2 = Calculate(2.5, 5.6);
            int res3 = Calculate(5, 6, 7);
            Console.WriteLine(res1);
            Console.WriteLine(res2);
            Console.WriteLine(res3);

            int num = 5;
            IncreamentVal(num);
            Console.WriteLine(num); // x remains 5

            int num1 = 5;
            IncreamentVal1(ref num1);
            Console.WriteLine(num1); // num1 becomes 6

            string input = "abc";
            bool success = TryParse(input, out int result);
            Console.WriteLine($"input: {input}, success: {success}, result: {result}");

            // Struct example
            Point p1 = new Point { X = 10, Y = 20 };
            Point p2 = p1;  // Copies the entire struct
            p2.X = 100;

            Console.WriteLine(p1.X);  // Still 10 (independent copy)
            Console.WriteLine(p2.X);  // 100

            // Class example
            Person p3 = new Person { Name = "Arjun", Age = 23 };
            Person p4 = p3; // Copies the reference
            p4.Name = "Siva";
            Console.WriteLine(p3.Name); // Siva
            Console.WriteLine(p4.Name); // Siva

            // Boxing and Unboxing example
            int num3 = 123;
            object obj = num3; // Boxing
            int num4 = (int)obj; // Unboxing

            // Avoid boxing in performance-critical code
            List<int> numbers = new List<int>();  // No boxing
            ArrayList oldList = new ArrayList();  // Boxing when adding int!

            // String manipulation example
            string s1 = "Arjun";
            string s2 = s1;
            s2 = "Siva";

            Console.WriteLine(s1); // Arjun
            Console.WriteLine(s2); // Siva

            // Efficient string concatenation using StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(i); // Efficient
            }
            string result3 = sb.ToString();
            Console.WriteLine(result3); // 0123456789

            // Array copy example
            int[] arr1 = { 1, 7, 9, 6, 5 };
            int[] arr2 = arr1; // Ref copy
            arr2[0] = 100;
            Console.WriteLine(arr1[0]); // 100

            // To create an independent copy
            int[] arr3 = (int[])arr1.Clone();
            int[] arr4 = arr1.ToArray();  // Using LINQ

            // Modify value example
            int x = 30;
            ModifyValue(x); // By value
            Console.WriteLine($"After modifying the value: {x}"); // x remains 30

            ModifyValueRef(ref x); // By reference
            Console.WriteLine($"After ModifyValueRef: {x}"); // x is modified to 50

            Console.WriteLine("\n=== Reference Type Demo ===");
            Person person = new Person { Name = "Alice", Age = 30 };
            ModifyPerson(person);
            Console.WriteLine($"After ModifyPerson: {person.Name}, {person.Age}");

            static void ModifyPerson(Person p)
            {
                p.Age = 31;  // Modifies original object
                p = new Person { Name = "Bob", Age = 25 };  // Only changes local reference
            }

            // TryParse demo
            Console.WriteLine("\n=== TryParse Pattern ===");
            Console.Write("Enter a number: ");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine($"You entered: {number}");
            }
            else
            {
                Console.WriteLine("Invalid number");
            }
        }
    }

    // VALUE TYPE - Stored on STACK
    struct Point  // Struct = value type
    {
        public int X;
        public int Y;
    }

    // Reference Type - Stored on HEAP
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}