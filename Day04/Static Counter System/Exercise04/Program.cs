using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
namespace Exercise04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"Total Users: {User.GetTotalUsers()}");
            User user1 = new User("Arjun");
            User user2 = new User("Siva");
            User user3 = new User("Sumanth");

            System.Console.WriteLine($"\nTotal Users: {User.GetTotalUsers()}");
            user1.Login();
            user2.Login();
            user3.Login();
            user2.Login();
            System.Console.WriteLine($"Total Logins Across all the users: {User.GetTotalLogins()}");

             Console.WriteLine($"\nUser details:");
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

             // Test IdGenerator
            Console.WriteLine($"\n=== ID Generator ===");
            Console.WriteLine(IdGenerator.GenerateUserId());
            Console.WriteLine(IdGenerator.GenerateUserId());
            Console.WriteLine(IdGenerator.GenerateProductId());
            Console.WriteLine(IdGenerator.GenerateOrderId());

        }
    }

    public class User
    {
        //static members 

        private static int totalUsers;
        private static int totalLogins;
        private static List<String> allUserNames = new List<string>();

        //Instance members
        public string Username{set; get;}
        public string UserId{set; get;}
        public int loginCount =0;
        public DateTime createdAt;

        public User(string username)
        {
            if (allUserNames.Contains(username))
            {
                throw new ArgumentException("User Already Exists");
            }
            Username = username;
            UserId = $"U{++totalUsers:D4}";
            createdAt = DateTime.Now;
            allUserNames.Add(username);
        }

        //instance Method
        public void Login()
        {
            loginCount++;
            totalLogins++;
            System.Console.WriteLine($"{Username} Logged in (login #{loginCount})");

            
        }

        //static methods
        public static int GetTotalUsers() => totalUsers;
        public static int GetTotalLogins() => totalLogins;

        public static bool IsUserNameTaken(string username)
        {
            return allUserNames.Contains(username);
        }

         
        public override string ToString()
        {
            return $"User: {Username} (ID: {UserId}, Created: {createdAt:yyyy-MM-dd}, Logins: {loginCount})";
        }
        
    }

    public static class IdGenerator
    {
        private static int userIdCounter = 0;
        private static int productIdCounter =0 ;
        private static int orderIdCounter = 0;

        public static string GenerateUserId()
        {
            return $"USR{++userIdCounter:D6}";
        }

        public static string GenerateProductId()
        {
            return $"PROD{++productIdCounter}";
        }
        public static string GenerateOrderId()
        {
            return $"ORD{++orderIdCounter}";
        }

        public static void ResetCounters()
        {
            userIdCounter = 0;
            productIdCounter = 0;
            orderIdCounter = 0;
        }
    }
}