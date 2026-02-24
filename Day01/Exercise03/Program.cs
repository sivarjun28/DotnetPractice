using System;

namespace Exercise03
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Simple Calculator ===\n");

            bool running = true;

            while (running)
            {
                DisplayMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PerformCalculation("Add");
                        break;
                    case "2":
                        PerformCalculation("Subtract");
                        break;
                    case "3":
                        PerformCalculation("Multiply");
                        break;
                    case "4":
                        PerformCalculation("Divide");
                        break;
                    case "5":
                        PerformCalculation("Power");
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            Console.WriteLine("4. Divide");
            Console.WriteLine("5. Power");
            Console.WriteLine("0. Exit");
            Console.Write("Enter choice: ");
        }

        static void PerformCalculation(string operation)
        {
            // Get the first number
            Console.Write("Enter first number: ");
            string? input1 = Console.ReadLine();
            bool isValid1 = double.TryParse(input1, out double num1);
            if (!isValid1)
            {
                Console.WriteLine("Invalid input for the first number.");
                return;
            }

            // Get the second number
            Console.Write("Enter second number: ");
            string? input2 = Console.ReadLine();
            bool isValid2 = double.TryParse(input2, out double num2);
            if (!isValid2)
            {
                Console.WriteLine("Invalid input for the second number.");
                return;
            }

            // Ask for precision (1=int, 2=double, 3=decimal)
            Console.Write("Choose precision (1=int, 2=double, 3=decimal): ");
            string? precisionChoice = Console.ReadLine();

            // Perform calculation based on chosen operation
            switch (precisionChoice)
            {
                case "1":
                    PerformIntCalculation(operation, num1, num2);
                    break;
                case "2":
                    PerformDoubleCalculation(operation, num1, num2);
                    break;
                case "3":
                    PerformDecimalCalculation(operation, num1, num2);
                    break;
                default:
                    Console.WriteLine("Invalid precision choice.");
                    break;
            }
        }

        static void PerformIntCalculation(string operation, double num1, double num2)
        {
            int result = 0;
            switch (operation)
            {
                case "Add":
                    result = (int)(num1 + num2);
                    break;
                case "Subtract":
                    result = (int)(num1 - num2);
                    break;
                case "Multiply":
                    result = (int)(num1 * num2);
                    break;
                case "Divide":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero.");
                        return;
                    }
                    result = (int)(num1 / num2);
                    break;
                case "Power":
                    result = (int)Math.Pow(num1, num2);
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    return;
            }
            Console.WriteLine($"Result (int): {result}");
        }

        static void PerformDoubleCalculation(string operation, double num1, double num2)
        {
            double result = 0;
            switch (operation)
            {
                case "Add":
                    result = num1 + num2;
                    break;
                case "Subtract":
                    result = num1 - num2;
                    break;
                case "Multiply":
                    result = num1 * num2;
                    break;
                case "Divide":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero.");
                        return;
                    }
                    result = num1 / num2;
                    break;
                case "Power":
                    result = Math.Pow(num1, num2);
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    return;
            }
            Console.WriteLine($"Result (double): {result}");
        }

        static void PerformDecimalCalculation(string operation, double num1, double num2)
        {
            decimal result = 0m;
            switch (operation)
            {
                case "Add":
                    result = (decimal)(num1 + num2);
                    break;
                case "Subtract":
                    result = (decimal)(num1 - num2);
                    break;
                case "Multiply":
                    result = (decimal)(num1 * num2);
                    break;
                case "Divide":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero.");
                        return;
                    }
                    result = (decimal)(num1 / num2);
                    break;
                case "Power":
                    result = (decimal)Math.Pow(num1, num2);
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    return;
            }
            Console.WriteLine($"Result (decimal): {result}");
        }
    }
}