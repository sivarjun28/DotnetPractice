using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a fleet of vehicles
            List<Vehicle> fleet = new()
            {
                new Car { Brand = "Toyota", Model = "Camry", Year = 2020, DailyRate = 50, NumberOfDoors = 4, FuelType = "Gasoline" },
                new Motorcycle { Brand = "Honda", Model = "CBR", Year = 2019, DailyRate = 30, EngineCC = 600 },
                new Truck { Brand = "Ford", Model = "F-150", Year = 2021, DailyRate = 80, LoadCapacity = 1000 },
                new ElectricCar { Brand = "Tesla", Model = "Model 3", Year = 2022, DailyRate = 70, NumberOfDoors = 4, Range = 350, BatteryCapacity = 75 }
            };

            // Display all available vehicles and cost for 10 days
            Console.WriteLine("Available Vehicles:");
            foreach (var vehicle in fleet.Where(v => !v.IsRented))
            {
                vehicle.DisplayInfo();
                Console.WriteLine($"Cost for 10 days: {vehicle.GetRentalCost(10):C}");
                Console.WriteLine();
            }

            // Rent the first vehicle
            fleet[0].Rent();
            Console.WriteLine();

            // Return it
            fleet[0].Return();
            Console.WriteLine();

            // Calculate total potential revenue for 10-day rentals
            decimal revenue = fleet.Sum(v => v.GetRentalCost(10));
            Console.WriteLine($"Total potential revenue (10 days): {revenue:C}");
        }
    }

    // Base class
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsRented { get; set; }

        public virtual bool Rent()
        {
            if (IsRented)
            {
                Console.WriteLine($"{Brand} {Model} is already rented");
                return false;
            }

            IsRented = true;
            Console.WriteLine($"{Brand} {Model} rented successfully");
            return true;
        }

        public virtual void Return()
        {
            IsRented = false;
            Console.WriteLine($"{Brand} {Model} returned");
        }

        public virtual decimal GetRentalCost(int days)
        {
            return DailyRate * days;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Brand} {Model} ({Year})");
            Console.WriteLine($"Daily Rate: {DailyRate:C}");
            Console.WriteLine($"Status: {(IsRented ? "Rented" : "Available")}");
        }
    }

    // Car class
    public class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public string FuelType { get; set; } = "Gasoline";

        public override decimal GetRentalCost(int days)
        {
            decimal cost = DailyRate * days;
            if (days > 7)
            {
                cost *= 0.9m; // 10% discount
            }
            return cost;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Doors: {NumberOfDoors}, Fuel: {FuelType}");
        }
    }

    // Motorcycle class
    public class Motorcycle : Vehicle
    {
        public int EngineCC { get; set; }

        public override decimal GetRentalCost(int days)
        {
            decimal cost = DailyRate * days;
            if (days > 3)
            {
                cost *= 0.85m; // 15% discount
            }
            return cost;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Engine: {EngineCC}cc");
        }
    }

    // Truck class
    public class Truck : Vehicle
    {
        public double LoadCapacity { get; set; } // kg

        public override decimal GetRentalCost(int days)
        {
            decimal extra = (decimal)(LoadCapacity * 0.1) * days;
            return DailyRate * days + extra;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Load Capacity: {LoadCapacity} kg");
        }
    }

    // ElectricCar class (inherits Car)
    public class ElectricCar : Car
    {
        public double BatteryCapacity { get; set; } // kWh
        public int Range { get; set; } // km

        public override decimal GetRentalCost(int days)
        {
            decimal baseCost = base.GetRentalCost(days);
            return baseCost * 0.8m; // 20% eco-friendly discount
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Battery: {BatteryCapacity} kWh, Range: {Range} km");
        }
    }
}