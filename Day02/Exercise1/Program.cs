using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Exercise1
{
    internal class Program
    {
        public static int Calculate(int a, int b)
        {
            return a + b; //Adding 2 integers 
        }
        public static double Calculate(double a , double b)
        {
            return a * b; // mulitplying two doubles
        }
        public static int Calculate(int a, int b, int c)
        {
            return a + b + c; // adding 3 integers
        }

         // Calculate(params int[]) - returns int (variable arguments)
         public static int Calculate(params int[] numbers)
        {
            int sum = 0;
            foreach(var num in numbers)
            {
                sum += num;
            }
            return sum;
        }
        public static string Format(double value, int decimals = 2, string prefix = "$")
        {
            return $"{prefix}{value.ToString($"F{decimals}")}";
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine(Calculate(23,56));
            System.Console.WriteLine(Calculate(78.90,1.23));
            System.Console.WriteLine(Calculate(6,8,9,7));
            System.Console.WriteLine(Calculate(1,8,1,10,20,1));

            Console.WriteLine(Format(88.98));
            Console.WriteLine(Format(123.456));                // Format(double, decimals=2, prefix="$")
            Console.WriteLine(Format(123.456, decimals: 1));    // Format(double, decimals=1)
            Console.WriteLine(Format(123.456, prefix: "&", decimals: 3));
        }
    }
}