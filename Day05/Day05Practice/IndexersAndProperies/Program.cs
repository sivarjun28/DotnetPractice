using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace IndexersAndProperies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person
            {
                Id = "76766",
                Email1 = "arjun@gmail.com"
            };
            // p.Id = "P002";  // ❌ Compile error (init-only)

            // Built-in indexer examples
            string text = "Hello";
            char firstChar = text[0];  // 'H' (string has indexer)

            List<int> numbers = new() { 1, 2, 3 };
            int first = numbers[0];  // 1 (List<T> has indexer)

            Dictionary<string, int> ages = new();
            ages["Alice"] = 30;  // Dictionary has indexer

            StudentGrades grades = new StudentGrades();
            grades["Math"] = 95;      // Uses indexer setter
            grades["Science"] = 87;
            int mathGrade = grades["Math"];  // Uses indexer getter
            Console.WriteLine(mathGrade);  // 95
                                           // Usage
            WeekSchedule myWeek = new();
            myWeek[DayOfWeek.Monday] = "Team Meeting";
            myWeek[DayOfWeek.Wednesday] = "Client Presentation";
            myWeek[DayOfWeek.Friday] = "Code Review";

            Console.WriteLine(myWeek[DayOfWeek.Monday]);  // "Team Meeting"
            Console.WriteLine(myWeek[DayOfWeek.Tuesday]); // "Free"

            Person1 p1 = new Person1
            {
                FirstName = "Siva",
                LastName = "Arjun",
                BirthDate = new DateTime(2002, 10, 28)
            };
            // p1.FirstName = "krishna";//compile error cannot re intialize
            // Usage
            Person2 person1 = new("Alice", "Smith", 30);
            Person2 person2 = new("Alice", "Smith", 30);

            Console.WriteLine(person1 == person2);  // true (value equality)
            Console.WriteLine(person1);  // Person { FirstName = Alice, LastName = Smith, Age = 30 }

            // With-expression (create copy with changes)
            Person2 person3 = person1 with { Age = 31 };

            // Must provide required properties
            User user = new User
            {
                Username = "alice123",
                Email = "alice@example.com"
                // PhoneNumber is optional
            };

            // ❌ Compile error - missing required properties
            // User user2 = new User { Username = "bob" };

            User1 user1 = new User1()
            {
                UserName = "Arjun",
                Email = "arjun@gmail.com",
                Age = 125,
                PhoneNumber = "961576576"
            };
            System.Console.WriteLine(user1.UserName);
            System.Console.WriteLine(user1.Age);
            System.Console.WriteLine(user1.Email);
            System.Console.WriteLine(user1.PhoneNumber);

        }
    }

    public class Person
    {
        private string _name;
        public string Name
        {
            get
            {
                System.Console.WriteLine("Getting name");
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name is required");
                System.Console.WriteLine($"Setting Name to {value}");
                _name = value;
            }
        }

        // EXPRESSION-BODIED PROPERTY (C# 7)
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.ToLower() ?? throw new ArgumentNullException();
        }


        // READ-ONLY PROPERTY (computed)
        // public string DisplayName => $"{FirstName} {LastName}";

        // PROPERTY with different access levels
        public int Age { get; private set; }

        // INIT-ONLY PROPERTY (C# 9) - can only be set during initialization
        public string Id { get; init; }

        // REQUIRED PROPERTY (C# 11) - must be set during initialization
        public required string Email1 { get; init; }
    }



    public class Product : INotifyPropertyChanged
    {
        // Lazy initialization for Tags
        private List<string>? _tags;
        public List<string> Tags
        {
            get
            {
                if (_tags == null)
                    _tags = LoadTagsFromDatabase();  // Lazy load tags when first accessed
                return _tags;
            }
        }

        // Cached property for TotalPrice
        private decimal? _totalPrice;
        public decimal TotalPrice
        {
            get
            {
                if (_totalPrice == null)
                    _totalPrice = CalculatePrice();  // Calculate once and cache the result
                return _totalPrice.Value;
            }
        }

        // Property with notification (change tracking) for Name
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));  // Notify listeners about the change
                }
            }
        }

        // Method for lazy loading tags from a database or external source
        private List<string> LoadTagsFromDatabase()
        {
            // Simulate database call
            return new List<string> { "Electronics", "Gadgets", "Sale" };
        }

        // Method to calculate the price of the product
        private decimal CalculatePrice()
        {
            // Simulate price calculation
            return 99.99m;
        }

        // Property changed notification
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // INotifyPropertyChanged interface implementation
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class StudentGrades
    {
        private Dictionary<string, int> grades = new();

        public int this[string subject]
        {
            get
            {
                if (grades.TryGetValue(subject, out int grade))
                    return grade;
                return 0;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Grade must be 0 - 100");
                }
                grades[subject] = value;
            }
        }

        // Can have multiple indexers with different parameter types
        public int this[int index]
        {
            get => grades.Values.ElementAt(index);
        }
    }
    //multi parameter indexer 
    public class Matrix
    {
        private int[,] data;
        public int this[int row, int col]
        {
            get => data[row, col];
            set => data[row, col] = value;
        }
    }

    public class WeekSchedule
    {
        private Dictionary<DayOfWeek, string> schedule = new();

        public string this[DayOfWeek day]
        {
            get => schedule.TryGetValue(day, out string? activity) ? activity : "Free";
            set => schedule[day] = value;
        }
    }

    // Immutable Objects
    public class Person1
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime BirthDate { get; init; }
    }
    //Record Types 
    // Record (shorthand for immutable class)
    public record Person2(string FirstName, string LastName, int Age);

    // Equivalent to:
    // public class Person2
    // {
    //     public string FirstName { get; init; }
    //     public string LastName { get; init; }
    //     public int Age { get; init; }

    //     // Plus: value-based equality, ToString, deconstruction
    // }

    // Required properties
    public class User
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public string? PhoneNumber { get; init; }  // Optional
    }

    public class PerformanceExample
{
    // FIELD - direct memory access (fastest)
    public int fieldValue;
    
    // AUTO-PROPERTY - compiler creates hidden field (minimal overhead)
    public int PropertyValue { get; set; }
    
    // FULL PROPERTY - method call overhead
    private int _value;
    public int FullPropertyValue
    {
        get { return _value; }
        set { _value = value; }
    }
}

// Performance: Field ≈ Auto-Property > Full Property
// But: Always prefer properties for public API (encapsulation, future flexibility)

public class User1
    {   
        [Required]
        [StringLength(50, MinimumLength =3)]
        public string UserName{get;set;}
        [EmailAddress]
        public string Email{get;set;}
        [Range(18,120)]
        public int Age{get;set;}
        [RegularExpression(@"^\d{10}$")]
        public string PhoneNumber{get;set;}
    }


    // DTO (Data Transfer Object) with records
public record ProductDto(int Id, string Name, decimal Price);

// Entity with validation
public class Product1
{
    public int Id { get; init; }
    
    private string _name;
    public required string Name
    {
        get => _name;
        init => _name = !string.IsNullOrWhiteSpace(value) 
            ? value 
            : throw new ArgumentException("Name required");
    }
    
    private decimal _price;
    public decimal Price
    {
        get => _price;
        set => _price = value >= 0 
            ? value 
            : throw new ArgumentException("Price must be non-negative");
    }
}

// Configuration with readonly
public class AppConfig
{
    public string ConnectionString { get; init; }
    public int MaxRetries { get; init; }
    public TimeSpan Timeout { get; init; }
}
}
