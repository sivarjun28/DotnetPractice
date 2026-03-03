using System;
namespace Exercise03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person
            {
                FirstName = "Siva",
                LastName = "Arjun",
                DateOfBirth = new DateTime(2002,10,28),
                Email = "arjunyadav6407@gmail.comj"
            };

            System.Console.WriteLine(p1);
            System.Console.WriteLine($"is Adult {p1.isAdult()}");
            

            //copying email
            Person p2 = p1 with {Email = "arjun.siva6407@gmail.com"};
            System.Console.WriteLine($"\n Original mail: {p1.Email}");
            System.Console.WriteLine($"\n modified mail: {p2.Email}");

            Person p3 = new Person

             {
                FirstName = "Siva",
                LastName = "Arjun",
                DateOfBirth = new DateTime(2002,10,28),
                Email = "arjunyadav6407@gmail.comj"
            };
            System.Console.WriteLine($"Person1 == person3{p1 == p3}");

            System.Console.WriteLine($"\n References Equal: {ReferenceEquals(p1,p3)}");

            // Demonstrating immutability by creating a new person (no direct modification allowed)
            try
            {
                // The following line will cause a compilation error since properties are immutable
                // p1.FirstName = "Bob";  // Cannot modify the property as it's immutable
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

             Person person4 = new Person
            {
                FirstName = "Bob",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1985, 8, 22),
                Email = "bob.johnson@example.com"
            };
            
            Console.WriteLine($"\nNew person: {person4}");
            
            // Demonstrating copying and modifying the new person
            Person person5 = person4 with { Email = "bob.j@example.com" };
            Console.WriteLine($"\nOriginal email: {person4.Email}");
            Console.WriteLine($"Modified copy email: {person5.Email}");
            
        }
    }

    public record Person
    {
        public required string FirstName {get; init;}
        public required string LastName {get; init;}
        public required DateTime DateOfBirth {get; init;}
        public required string Email{get; init;}

        public string FullName =>  $"{FirstName} {LastName}";

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if(DateOfBirth.Date > today.AddYears(-age))
                    age--;
                return age;
            }
        }
        public bool isAdult() => Age > 18 ;

        // Custom ToString (or use default record ToString)
        public override string ToString()
        {
            return $"{FullName} (Age: {Age}, Email: {Email})";
        }
    }
}
