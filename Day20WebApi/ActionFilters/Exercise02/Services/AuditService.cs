using Exercise02.Filters;
using Exercise02.Models;
using System.Threading.Tasks;

namespace Exercise02.Services
{
    public class AuditService : IAuditService
    {
        public Task LogAsync(AuditEntry entry)
        {
           
            Console.WriteLine("----- Audit Log -----");
            Console.WriteLine($"Timestamp : {entry.Timestamp}");
            Console.WriteLine($"User      : {entry.User}");
            Console.WriteLine($"Controller: {entry.Controller}");
            Console.WriteLine($"Action    : {entry.Action}");
            Console.WriteLine($"Status    : {entry.StatusCode}");
            Console.WriteLine($"Duration  : {entry.DurationMs} ms");
            if (entry.Exception != null)
                Console.WriteLine($"Exception : {entry.Exception}");

            if (entry.Parameters.Count > 0)
            {
                Console.WriteLine("Parameters:");
                foreach (var kvp in entry.Parameters)
                    Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
            }

            Console.WriteLine("--------------------");

            return Task.CompletedTask;
        }
    }
}