using System;
using System.Security.Cryptography.X509Certificates;
namespace Exercise04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppConfig appConfig = new AppConfig
            {
                AppName = "Vsc Application",
                Version = "18.0.0",
                Database = new DatabaseConfig
                {
                    ConnectionString = "Server=localhost;Database=MyDb;",
                    MaxConnections = 100,
                    Timeout = TimeSpan.FromSeconds(45)
                },
                Api = new ApiConfig
                {
                    BaseUrl = "https://api.example.com",
                    ApiKey = "sivarjun1228@@",
                    RateLimitPerMinute= 1000
                }
            };

            if(appConfig.ValidateConfiguration()){
                appConfig.DisplayConfiguration();
            }
            else
            {
                System.Console.WriteLine("Configuration Validation Failed");
            }
        }
    }

    public record DatabaseConfig
    {
        public required string ConnectionString { get; init; }
        private int _maxConnections;
        public required int MaxConnections
        {
            get => _maxConnections;
            init
            {
                if (value <= 0 || value >= 1000)
                {
                    throw new ArgumentException("Max connections must be 1 to 1000");
                }
                _maxConnections = value;
            }
        }

        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    }

    public record ApiConfig
    {
        public required string BaseUrl { get; init; }
        public required string ApiKey { get; init; }
        public int RateLimitPerMinute { get; init; } = 60;
    }

    public class AppConfig
    {
        public required DatabaseConfig Database { get; init; }
        public required ApiConfig Api { get; init; }
        public required string AppName { get; init; }
        public string Version { get; init; } = "18.0.0";

        public bool ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(AppName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Database.ConnectionString))
            {
                return false;
            }
            if (Database.MaxConnections <= 0 || Database.MaxConnections >= 1000)
            {
                return false;
            }
            if (Database.Timeout <= TimeSpan.Zero)
            {
                return false;
            }
            // Validate Api Configuration
            if (string.IsNullOrWhiteSpace(Api.ApiKey))
                return false;

            if (Api.RateLimitPerMinute <= 0 || Api.RateLimitPerMinute >= 1000)
                return false;  // Ensure RateLimitPerMinute is within a valid range (you can adjust the range as necessary)

            // Optional: Validate Version format (if you want to follow a semantic versioning pattern)
            var versionRegex = new System.Text.RegularExpressions.Regex(@"^\d+\.\d+\.\d+$");
            if (!versionRegex.IsMatch(Version))
                return false;  // Validate semantic versioning

            return true;
        }

        public void DisplayConfiguration()
        {
            System.Console.WriteLine("====Application Configuration====");
            System.Console.WriteLine($"Appname: {AppName} v{Version}");
            System.Console.WriteLine($"\n Database");
            System.Console.WriteLine($" Connection: {Database.ConnectionString}");
            System.Console.WriteLine($" MaxConnections: {Database.MaxConnections}");
            System.Console.WriteLine($" Timeout: {Database.Timeout}");
            Console.WriteLine($"  Base URL: {Api.BaseUrl}");
            Console.WriteLine($"  Rate Limit: {Api.RateLimitPerMinute}/min");
        }
    }


}
