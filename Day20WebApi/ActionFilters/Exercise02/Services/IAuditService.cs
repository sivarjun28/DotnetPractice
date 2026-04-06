using Exercise02.Models;

namespace Exercise02.Services
{
    public interface IAuditService
    {
        Task LogAsync(AuditEntry entry);
    }
}