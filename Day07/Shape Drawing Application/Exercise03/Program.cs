using System;
using System.Data.SqlTypes;
namespace Exercise03
{

    /*
Abstract class: Shape
- Properties: Color, Position (X, Y)
- Abstract: GetArea(), GetPerimeter()
- Virtual: Draw()

Interfaces:
1. IDrawable: Draw()
2. IResizable: Resize(factor)
3. IRotatable: Rotate(degrees)
4. ISelectable: Select(), Deselect(), IsSelected

Shapes:
1. Circle: Shape, IDrawable, IResizable
2. Rectangle: Shape, IDrawable, IResizable, IRotatable
3. Triangle: Shape, IDrawable, IRotatable
4. Line: Shape, IDrawable, IRotatable (Area/Perimeter = 0)
*/
    public abstract class Shape
    {
        public string Color { get; set; } = "Black";
        public double X { get; set; }
        public double Y { get; set; }
        public abstract double GetArea();
        public abstract double GetPerimeter();
        public virtual void Draw()
        {
            System.Console.WriteLine($"Drawing {GetType().Name} at ({X}, {Y}) in {Color}");
        }

    }
    public interface IDrawable
    {
        void Draw();
    }
    public interface IResizable
    {
        void Resize(double factor);
    }
    public interface IRotatable
    {
        void Rotate(double degrees);
    }
    public interface ISelectable
    {
        bool IsSelected { get; }
        void Select();
        void Deselect();
    }
    public class Circle : Shape, IDrawable, IResizable,  ISelectable
    {
        public double Radius { get; set; }
        public bool IsSelected { get; private set; }

        public override double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
        public override double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }

        public void Resize(double factor)
        {
            Radius *= factor;
            System.Console.WriteLine($"Cirle resized to radius {Radius} ");
        }
        public void Select()
        {
            IsSelected = true;
            Console.WriteLine("Circle selected");
        }

        public void Deselect()
        {
            IsSelected = false;
            Console.WriteLine("Circle deselected");
        }
    }
    public class Rectangle : Shape, IDrawable, IResizable, IRotatable
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public override double GetArea()
        {
            return Width * Height;
        }

        public override double GetPerimeter()
        {
            return 2 * (Width + Height);
        }

        public void Resize(double factor)
        {
            Width *= factor;
            Height *= factor;
            Console.WriteLine($"Rectangle resized to {Width}x{Height}");
        }

        public void Rotate(double degrees)
        {
            Console.WriteLine($"Rectangle rotated by {degrees}°");
        }
    }

   
    public class Triangle : Shape, IDrawable, IRotatable
    {
        public double Base { get; set; }
        public double Height { get; set; }
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }

        public override double GetArea()
        {
            return 0.5 * Base * Height;
        }

        public override double GetPerimeter()
        {
            return Side1 + Side2 + Side3;
        }

        public void Rotate(double degrees)
        {
            Console.WriteLine($"Triangle rotated by {degrees}°");
        }
    }
    public class Line : Shape, IDrawable, IRotatable
    {
        public double Length { get; set; }

        public override double GetArea()
        {
            return 0;  // A line doesn't have an area.
        }

        public override double GetPerimeter()
        {
            return Length;
        }

        public void Rotate(double degrees)
        {
            Console.WriteLine($"Line rotated by {degrees}°");
        }
    }

    public class Canvas
    {
        private List<Shape> shapes = new();
        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
            System.Console.WriteLine($"added {shape.GetType().Name}");
        }

        public void DrawAll()
        {
            System.Console.WriteLine("-----Drawing Canvas----");
            foreach (var shape in shapes)
            {
                shape.Draw();
                System.Console.WriteLine($" Area: {shape.GetArea():F2}, Perimeter: {shape.GetPerimeter():F2}");
            }
        }
        public void ResizeAll(double factor)
        {
            Console.WriteLine($"\n--- Resizing all shapes by {factor}x ---");
            foreach (var shape in shapes)
            {
                if (shape is IResizable resizable)
                {
                    resizable.Resize(factor);
                }
            }
        }
        public void RotateAll(double degrees)
        {
            Console.WriteLine($"\n--- Rotating all shapes by {degrees}° ---");
            foreach (var shape in shapes)
            {
                if (shape is IRotatable rotatable)
                {
                    rotatable.Rotate(degrees);
                }
            }
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Canvas canvas = new();

            canvas.AddShape(new Circle { X = 10, Y = 10, Radius = 5, Color = "Red" });
            canvas.AddShape(new Rectangle { X = 20, Y = 20, Width = 10, Height = 5, Color = "Blue" });
            canvas.AddShape(new Triangle { X = 30, Y = 30, Base = 10, Height = 8, Color = "Green" });

            canvas.DrawAll();
            canvas.ResizeAll(1.5);
            canvas.RotateAll(45);
            canvas.DrawAll();

        }
    }
}
