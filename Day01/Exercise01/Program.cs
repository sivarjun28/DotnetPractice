using System;

namespace Exercise01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Type System Exploration ===\n");
            
            // TODO: Declare and display byte range
            byte minByte = byte.MinValue;
            byte maxByte = byte.MaxValue;
            Console.WriteLine($"byte: {minByte} to {maxByte}");
            
            // TODO: Continue for int, long, float, double, decimal
            int minInt = int.MinValue;
            int maxInt = int.MaxValue;
            System.Console.WriteLine($"int: {minInt} to {maxInt}");

            long minLong = long.MinValue;
            long maxLong = long.MaxValue;
            System.Console.WriteLine($"long: {minLong} to {maxLong}");

            float minFloat = float.MinValue;
            float maxFloat = float.MaxValue;
            System.Console.WriteLine($"float: {minFloat} to {maxFloat}");

            double minDouble = double.MinValue;
            double maxDouble = double.MaxValue;
            System.Console.WriteLine($"double: {minDouble} to {maxDouble}");

            decimal minDecimal = decimal.MinValue;
            decimal maxDecimal = decimal.MaxValue;
            System.Console.WriteLine($"decimal: {minDecimal} to {maxDecimal}");
            // TODO: Arithmetic operations
            int a = 10;
            int b = 3;
            Console.WriteLine($"\n=== Integer Division ===");
            Console.WriteLine($"{a} / {b} = {a / b}");  // Integer division
            Console.WriteLine($"{a} / (double){b} = {a / (double)b}");  // Float division

            double res1 = a + 5.5;
            System.Console.WriteLine($"\n=== Mixed Addition ===");
            System.Console.WriteLine($"{a} + 5.5 = {res1}");

            decimal res2 = 12.5m + 10.0m;
            System.Console.WriteLine($"\n=== Decimal Addition ===");
            System.Console.WriteLine($"12.5m + 10.m = {res2}");
            // TODO: Type casting examples
            int smallInt = 100;
            long longInt = smallInt;
            System.Console.WriteLine($"Implicit casting : int {smallInt} to long {longInt}");
            
            long largeLong = 5000;
            int largeInt = (int)largeLong;
            System.Console.WriteLine($"Explicit casting: long {largeLong} to int {largeInt}");

            double doubleValue = 99.60;
            int intValue = (int)doubleValue;
            System.Console.WriteLine($"Explicit casting: double {doubleValue} to int {intValue}");
        }
    }
}