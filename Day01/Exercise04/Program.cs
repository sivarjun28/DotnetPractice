using System;

namespace Exercise04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== User Registration ===\n");
            
            // Required fields
            Console.Write("Username (required): ");
            string? username = Console.ReadLine();
            
            Console.Write("Email (required): ");
            string? email = Console.ReadLine();
            
            // Optional fields
            Console.Write("Age (optional, press Enter to skip): ");
            string? ageInput = Console.ReadLine();
            int? age = string.IsNullOrWhiteSpace(ageInput) ? null : int.Parse(ageInput);
            
            // Phone (optional)
            Console.Write("Phone (optional, press Enter to skip): ");
            string? phone = Console.ReadLine();
            
            // Middle Name (optional)
            Console.Write("Middle Name (optional, press Enter to skip): ");
            string? middleName = Console.ReadLine();
            
            // Display summary using null-coalescing
            Console.WriteLine("\n=== Registration Summary ===");
            Console.WriteLine($"Username: {username ?? "N/A"}");
            Console.WriteLine($"Email: {email ?? "N/A"}");
            Console.WriteLine($"Age: {age?.ToString() ?? "Not provided"}");
            Console.WriteLine($"Phone: {phone ?? "Not provided"}");
            Console.WriteLine($"Middle Name: {middleName ?? "Not provided"}");

            // Validation (username and email are required)
            bool isValid = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(email);
            
            Console.WriteLine($"\nRegistration {(isValid ? "successful" : "failed")}");
        }
    }
}