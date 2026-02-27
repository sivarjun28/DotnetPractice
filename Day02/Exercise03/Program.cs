using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Exercise03
{
    internal class Program
    {
        static double CalculateRectangleArea()
        {
            System.Console.Write("Enter the length ");
            double length;
            
            {
                throw new FormatException("Invalid Length. Please Enter the positive length");
            }
            System.Console.WriteLine("Enter the Width");


            double width;
            if (!double.TryParse(Console.ReadLine(), out width) || (width <= 0))
            {
                throw new FormatException("Invalid width. Please Enter the positive width");
            }
            return length * width;
        }
        static double CalculateCircleArea()
        {
            System.Console.WriteLine("Enter the radius ");
            double radius;
            if (double.TryParse(Console.ReadLine(), out radius) && (radius > 0))
            {
                return Math.PI * radius * radius;
            }
            else
            {
                throw new FormatException("Invalid input, Enter the positive radius");
            }
        }

        static double CalculateTriangleArea()
        {
            System.Console.WriteLine("Enter the base ");
            double baseLength;
            if (!double.TryParse(Console.ReadLine(), out baseLength) || (baseLength <= 0))
            {
                throw new FormatException("Invalid input, Please Enter the positive base ");
            }
            System.Console.WriteLine("Enter the height ");
            double height;
            if (!double.TryParse(Console.ReadLine(), out height) || (height <= 0))
            {
                throw new FormatException("Invalid input, Please Enter the positive height ");
            }
            return 0.5 * baseLength * height;
        }
         static string DescribeShape(Object shape) => shape switch
            {
                Circle c when c.Radius < 0 => "Invalid Choice",
                Circle c => $"circle with radius {c.Radius}",
                Rectangle r => $"Rectangle {r.Width} x {r.Length}"
            };

        static void Main(string[] args)
        {
            System.Console.WriteLine("shape Clculator");
            System.Console.WriteLine("1. Rectamgle");
            System.Console.WriteLine("2. Circle");
            System.Console.WriteLine("3. Triangle");
            System.Console.WriteLine("Enter choice:");

            string? choice = Console.ReadLine();
            double area = choice switch
            {
                "1" => CalculateRectangleArea(),
                "2" => CalculateCircleArea(),
                "3" => CalculateTriangleArea(),
                _ => throw new InvalidOperationException("Invalid choice")
            };

            System.Console.WriteLine($"Area: {area:F2}");
            Circle circle = new Circle(5);
            Console.WriteLine(DescribeShape(circle));


            Rectangle rectangle = new Rectangle(4, 6);
            Console.WriteLine(DescribeShape(rectangle));

           
        }

    }
    public class Circle
    {
        public double Radius { set; get; }
        public Circle(double radius)
        {
            Radius = radius;
        }
    }
    public class Rectangle
    {
        public double Length { set; get; }
        public double Width { set; get; }
        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }
    }
}
