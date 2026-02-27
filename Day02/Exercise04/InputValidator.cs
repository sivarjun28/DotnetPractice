using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Exercise04
{
    internal class InputValidator
    {
        public static bool TryParseEmail(string input, out string email)
        {
            //Email validation
            email = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            
                return false;
            
            
            if(input.Contains("@") && input.Contains("."))
            {
                email = input.Trim().ToLower();
                return true;
            }
            return false;

           

        } 
         //Age Validation
         public static bool TryParseAge(string input, out int age)
        {
            age = 0;
            if(!int.TryParse(input, out age))
                return false;
            return age >=1 && age <= 120;
        }
         //Phone Validation
         public static bool TryParsePhoneNumber(string input, out string phoneNumber)
        {
            phoneNumber = string.Empty;
            string cleanedInput = new string(input.Where(c => Char.IsDigit(c)).ToArray());
            if(cleanedInput.Length == 10)
            {
                phoneNumber = cleanedInput;
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("===Registration Form===");
            System.Console.Write("Enter email ");
            string? emailInput = Console.ReadLine();
            if(TryParseEmail(emailInput ?? "", out string email))
            {
                System.Console.WriteLine($"Valid email: {email}");
            }
            else
            {
                System.Console.WriteLine($" Invalid email format");
            }

            System.Console.Write("Enter Age ");
            string? ageInput = Console.ReadLine();
            if(TryParseAge(ageInput ?? "", out int age))
            {
                System.Console.WriteLine($"valid age {ageInput}");
            }
            else
            {
                System.Console.WriteLine("Invalid Age Format(must in between 1 - 120)");
            }

            System.Console.WriteLine("Enter Phonenumber ");
            string phoneNumberInput = Console.ReadLine();
            if(TryParsePhoneNumber(phoneNumberInput ?? "", out string phoneNumber))
            {
                System.Console.WriteLine($"valid Phone Number {phoneNumberInput}");
            }
            else
            {
                System.Console.WriteLine($"Invalid Phone number Format (must conatin 10 numbers)");
            }
        }
    }
}