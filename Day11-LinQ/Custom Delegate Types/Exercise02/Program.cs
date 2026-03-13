using System;
using System.Collections.Generic;

namespace Exercise02
{
    // Custom delegate types
    public delegate double MathOperation(double a, double b);
    public delegate double UnaryOperation(double value);

    public class Calculator
    {
        private double currentValue = 0;
        private Stack<double> history = new();
        private Dictionary<string, MathOperation> operations = new();

        public double CurrentValue => currentValue;

        public Calculator()
        {
            // Register default operations
            RegisterOperation("add", (a, b) => a + b);
            RegisterOperation("subtract", (a, b) => a - b);
            RegisterOperation("multiply", (a, b) => a * b);
            RegisterOperation("divide", (a, b) => b != 0 ? a / b : throw new DivideByZeroException());
        }

        public void RegisterOperation(string name, MathOperation operation)
        {
            operations[name.ToLower()] = operation;
        }

        // Return Calculator to allow chaining
        public Calculator Set(double value)
        {
            history.Push(currentValue);
            currentValue = value;
            return this;
        }

        public void Execute(string operationName, double operand)
        {
            if (!operations.TryGetValue(operationName.ToLower(), out MathOperation? operation))
                throw new ArgumentException($"Unknown operation: {operationName}");

            history.Push(currentValue);
            currentValue = operation(currentValue, operand);

            Console.WriteLine($"{operationName}({operand}) = {currentValue}");
        }

        // Implement Undo
        public void Undo()
        {
            if (history.Count > 0)
            {
                currentValue = history.Pop();
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }

        // Apply unary operations
        public void Apply(UnaryOperation operation)
        {
            history.Push(currentValue);
            currentValue = operation(currentValue);
        }

        // Fluent interface for chaining
        public Calculator Chain(Func<double, double> operation)
        {
            history.Push(currentValue);
            currentValue = operation(currentValue);
            return this;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();

            calc.Set(10);
            calc.Execute("add", 5);       // 15
            calc.Execute("multiply", 2);  // 30
            calc.Execute("subtract", 10); // 20

            Console.WriteLine($"Current: {calc.CurrentValue}");

            calc.Undo();
            Console.WriteLine($"After undo: {calc.CurrentValue}");

            // Register custom operation
            calc.RegisterOperation("power", Math.Pow);
            calc.Execute("power", 2);

            // Unary operations
            calc.Apply(Math.Sqrt);
            Console.WriteLine($"After sqrt: {calc.CurrentValue}");

            // Chaining operations
            calc.Set(10)
                .Chain(x => x * 2)
                .Chain(x => x + 5)
                .Chain(Math.Sqrt);

            Console.WriteLine($"After chaining: {calc.CurrentValue}");
        }
    }
}