using System;
using System.Security.Cryptography.X509Certificates;
namespace Exercise02
{
    internal class Program{
    static void Main(string[] args)
    {
        // Test
    List<Shape> shapes = new()
    {
    new Rectangle(5, 10, "Blue"),
    new Circle(7, "Red"),
    new Triangle(6, 4, 5, 5, 6, "Green"),
    new Square(4, "Yellow")
    };

        foreach (var shape in shapes)
        {
            shape.Display();
            Console.WriteLine();
        }
    }
    }

    public class Shape
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public Shape(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public virtual double GetArea()
        {
            return 0;
        }

        public virtual double GetPerimeter()
        {
            return 0;
        }

        public void Display()
        {
            Console.WriteLine($"{Name} ({Color})");
            Console.WriteLine($"Area: {GetArea():F2}");
            Console.WriteLine($"Perimeter: {GetPerimeter():F2}");
        }
    }

    // Implement Rectangle
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height, string color)
            : base("Rectangle", color)
        {
            Width = width;
            Height = height;
        }

        // Override GetArea and GetPerimeter for Rectangle
        public override double GetArea()
        {
            return Width * Height;
        }

        public override double GetPerimeter()
        {
            return 2 * (Width + Height);
        }
    }

    // Implement Circle
    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius, string color)
            : base("Circle", color)
        {
            Radius = radius;
        }

        // Override GetArea and GetPerimeter for Circle
        public override double GetArea()
        {
            return Math.PI * Radius * Radius;
        }

        public override double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }
    }

    // Implement Triangle
    public class Triangle : Shape
    {
        public double Base { get; set; }
        public double Height { get; set; }
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }

        public Triangle(double baseLength, double height, double side1, double side2, double side3, string color)
            : base("Triangle", color)
        {
            Base = baseLength;
            Height = height;
            Side1 = side1;
            Side2 = side2;
            Side3 = side3;
        }

        // Override GetArea and GetPerimeter for Triangle
        public override double GetArea()
        {
            return 0.5 * Base * Height; // Simple area formula for a triangle
        }

        public override double GetPerimeter()
        {
            return Side1 + Side2 + Side3; // Perimeter is the sum of all three sides
        }
    }

    // Implement Square (inherits from Rectangle)
    public class Square : Rectangle
    {
        public Square(double side, string color)
            : base(side, side, color) // Calls the Rectangle constructor with equal width and height
        {
            Name = "Square"; // Override the name to display "Square"
        }
    }

}
