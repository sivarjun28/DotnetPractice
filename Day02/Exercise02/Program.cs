using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Exercise02
{
    struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
       
    }

    
    public class Rectangle()
    {
        public int Width {set; get;}
        public int Height {set; get;}
        public int Area() => Width * Height;

        public override string ToString() => $"Rectangele {Width} x {Height}";

        
    }
    internal class Program
    {
        public static void MovePoint(ref Point point, int d1, int d2)
        {
            point.X += d1;
            point.Y += d2;
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("===Value Type Behavior===");
            Point p1 = new Point(10,20);
            Point p2 = p1;
            p2.X = 24;
            System.Console.WriteLine($"p1: {p1}");
            System.Console.WriteLine($"p2: {p2}");

            System.Console.WriteLine("===Ref type Behavior===");
            Rectangle r1 = new Rectangle{Width = 10 , Height = 20};
            Rectangle r2 = r1;
            System.Console.WriteLine($"r1: {r1}");
            System.Console.WriteLine($"r2: {r2}");

            System.Console.WriteLine("===using ref Parameter===");
            Point p3 = new Point(5,5);
            System.Console.WriteLine($"Before p3 : {p3}");
            MovePoint(ref p3, 12,12);
            System.Console.WriteLine($"After p3 :{p3}");

            // Add array demonstration Ref Type
            System.Console.WriteLine("\n=== Array Reference Type Behavior ===");

            int[] arr1 = {2,3,6,7,8,9};
            int[] arr2 = arr1;

            arr2[0] = 89;
            System.Console.WriteLine($"arr1 : {string.Join(",",arr1)}");
            System.Console.WriteLine($"arr1 : {string.Join(",",arr2)}");
        }
    }
}
