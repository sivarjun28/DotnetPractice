using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
namespace Exercise03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Temperature temp1 = new Temperature(25, 'C');
            System.Console.WriteLine(temp1);

            Temperature temp2 = Temperature.FromFahrenheit(98.9);
            System.Console.WriteLine($"Body Temperature: {temp2}");

            Temperature temp3 = Temperature.FromKelvin(300);
            System.Console.WriteLine(temp3);

            // Test validation
            try
            {
                Temperature invalid = new Temperature(-100, 'C');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public class Temperature
    {
        private const double AbsoluteZeroCelsius = -273.15;
        private double celsius;

        public double Celsius
        {
            get => celsius;
            set
            {
                if (value < AbsoluteZeroCelsius)
                    throw new ArgumentException(($"Temperature cannot be below absolute zero ({AbsoluteZeroCelsius}°C)"));
                celsius = value;
            }
        }

         public double Fahrenheit
        {
            get => (Celsius * 9 / 5) + 32;
            set => Celsius = (value - 32) * 5 / 9;
        }
        
        public double Kelvin
        {
            get => Celsius + 273.15;
            set => Celsius = value - 273.15;
        }

        public Temperature(double value, char unit = 'C')
        {
            switch (char.ToUpper(unit))
            {
                case 'C':
                     Celsius = value;
                     break;
                case 'F':
                    Fahrenheit = value;
                    break;
                case 'K':
                    Kelvin = value;
                    break;
                default:
                    throw new ArgumentException("Unit must be C, F or K");
            }
        }

        // Static factory methods
        public static Temperature FromCelsius(double celsius)
        {
            return new Temperature(celsius, 'C');
        }
        
        public static Temperature FromFahrenheit(double fahrenheit)
        {
            return new Temperature(fahrenheit, 'F');
        }
        
        public static Temperature FromKelvin(double kelvin)
        {
            return new Temperature(kelvin, 'K');
        }
        
        public override string ToString()
        {
            return $"{Celsius:F2}°C = {Fahrenheit:F2}°F = {Kelvin:F2}K";
        }
    
    }
}
