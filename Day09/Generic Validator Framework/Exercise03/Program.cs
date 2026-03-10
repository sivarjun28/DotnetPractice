using System;
namespace Exercise03
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }

    public interface IValidator<T>
    {
        ValidationResult Validate(T instance);
    }

   
    public class Validator<T> : IValidator<T>
    {
        private readonly List<(Func<T, bool> rule, string errorMessage)> rules = new();

        public Validator<T> AddRule(Func<T, bool> rule, string errorMessage)
        {
            rules.Add((rule, errorMessage));
            return this;
        }

        public Validator<T> Required(Func<T, string?> property, string fieldName)
        {
            return AddRule(
                obj => !string.IsNullOrWhiteSpace(property(obj)),
                $"{fieldName} is required"
            );
        }

        public Validator<T> Range(Func<T, int> property, int min, int max, string fieldName)
        {
            return AddRule(
                obj =>
                {
                    int value = property(obj);
                    return value >= min && value <= max;
                },
                $"{fieldName} must be between {min} and {max}"
            );
        }

        public Validator<T> Email(Func<T, string> property, string fieldName)
        {
            return AddRule(
                obj =>
                {
                    string email = property(obj);
                    return email.Contains('@') && email.Contains('.');
                },
                $"{fieldName} must be a valid email"
            );
        }

        public ValidationResult Validate(T instance)
        {
            ValidationResult result = new();

            foreach (var (rule, errorMessage) in rules)
            {
                if (!rule(instance))
                {
                    result.AddError(errorMessage);
                }
            }

            return result;
        }
    }

    // TODO: Test models
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Test
            Validator<User> userValidator = new Validator<User>()
                .Required(u => u.Username, "Username")
                .Required(u => u.Email, "Email")
                .Email(u => u.Email, "Email")
                .Range(u => u.Age, 18, 120, "Age")
                .AddRule(u => u.Password.Length >= 8, "Password must be at least 8 characters");

            User user = new()
            {
                Username = "Sivarjun28",
                Email = "arjunyadav5407@gmail.com",
                Age = 12,
                Password = "12345678@"
            };

            ValidationResult result = userValidator.Validate(user);

            if (!result.IsValid)
            {
                Console.WriteLine("Validation failed:");
                foreach (string error in result.Errors)
                {
                    Console.WriteLine($"- {error}");
                }
            }
            
        }
    }

}
