using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace InheritancePolymorphism
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //usage
            Dog dog = new Dog();
            dog.Name = "snopy";
            dog.Age = 2;
            dog.Breed = "German shephard";
            dog.Eat();//from parent
            dog.Sleep();//from parent
            dog.Bark();//from dog class

            Dog1 dog1 = new Dog1("snoopy", 2, "Dabourman");

            Animal2 danimal2 = new Dog2 { Name = "Snoopy" };
            Animal2 canimal2 = new Cat { Name = "pinky" };
            danimal2.MakeSound();
            canimal2.MakeSound();

            List<Animal2> animals = new()
            {
                new Dog2{Name = "Buddy"},
                new Cat{Name = "milky"},
                new Animal2 {Name = "Generic"}
            };
            foreach (Animal2 animal in animals)
            {
                animal.MakeSound();
                animal.Move();
            }

            Dog2 dog2 = new Dog2();
            dog2.Eat();

            DerivedClass derivedClass = new DerivedClass();
            derivedClass.Method1(); //Derived class method1
            derivedClass.Method2(); //Derived class method 2

            BaseClass baseRef = new DerivedClass();
            baseRef.Method1(); // "DerivedClass Method1" (override - polymorphic)
            baseRef.Method2();  // "BaseClass Method2" (new - NOT polymorphic)

            SavingsAccount bankAccount = new SavingsAccount();
            System.Console.WriteLine(bankAccount.AccountNumber);
            bankAccount.AddInterest();

            // Usage
            // Shape shape = new Shape();  // ❌ Cannot instantiate abstract class
            Shape circle = new Circle { Name = "Circle", Radius = 5 };
            circle.Display();
        }
    }

    //Part 1: Inheritance Basics
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public void Eat()
        {
            System.Console.WriteLine($"{Name} is Eating");
        }


        public void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping");
        }
    }

    public class Dog : Animal
    {
        public string Breed { get; set; }
        public void Bark()
        {
            System.Console.WriteLine($"{Name} says : boof");
        }
    }

    //Constructor Chaining

    public class Animal1
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Animal1(string name, int age)
        {
            Name = name;
            Age = age;
            System.Console.WriteLine("Animall Constructor called");
        }
    }

    public class Dog1 : Animal1
    {
        public string Breed { get; set; }
        // Must call base constructor
        public Dog1(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
            System.Console.WriteLine("Dog1 constructor called");
        }
    }

    //Polymorphism 

    public class Animal2
    {
        public string Name { get; set; }

        public virtual void MakeSound()
        {
            System.Console.WriteLine("some generic animal sound");
        }
        public virtual void Move()
        {
            System.Console.WriteLine($"{Name} is moving");
        }

        public virtual void Eat()
        {
            System.Console.WriteLine("Eating....");
        }
    }
    public class Dog2 : Animal2
    {
        public override void MakeSound()
        {
            System.Console.WriteLine($"{Name} says : Woof");
        }
        public override void Move()
        {
            System.Console.WriteLine($"{Name} is running");
        }
        public override void Eat()
        {
            base.Eat();//calling base implementation
            System.Console.WriteLine("Dog is munching on dog food");
        }
    }
    public class Cat : Animal2
    {
        public override void MakeSound()
        {
            System.Console.WriteLine($"{Name} says : Meow");
        }
        public override void Move()
        {
            System.Console.WriteLine($"{Name} is runnin");
        }
    }

    // Override vs New (Method Hiding)
    public class BaseClass
    {
        public virtual void Method1()
        {
            System.Console.WriteLine("Base Class method1");
        }
        public void Method2()
        {
            System.Console.WriteLine("Base class Method2");
        }
    }
    public class DerivedClass : BaseClass
    {
        public override void Method1()
        {
            System.Console.WriteLine("DerivedClass Method1");
        }
        // NEW (hiding) - hides base method
        public new void Method2()
        {
            System.Console.WriteLine("Derived class Method2");
        }
    }

    //Access Modifiers in Inheritance
    public class BankAccount
    {
        private decimal balance;  // Only accessible in BankAccount
        protected decimal interestRate;  // Accessible in derived classes
        public string AccountNumber { get; set; }

        protected decimal Balance => balance;  // Protected property

        protected void UpdateBalance(decimal amount)
        {
            balance += amount;
        }
    }

    public class SavingsAccount : BankAccount
    {
        public void AddInterest()
        {
            // Can access protected members
            decimal interest = Balance * interestRate;
            UpdateBalance(interest);
            Console.WriteLine($"Added interest: {interest:C}");

        }

        // Cannot access private members
        // balance = 100;  // ❌ Compile error
    }

    //Sealed Classes and Methods
    public sealed class Configuration
    {
        public string Setting { get; set; }
    }
    // ❌ Cannot inherit from sealed class
    // public class MyConfig : Configuration { }  // Compile error

    public abstract class Shape
    {
        public string Name { get; set; }

        public abstract double CalculateArea();

        public void Display()
        {
            System.Console.WriteLine($"{Name} - Area {CalculateArea()}");
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

}
