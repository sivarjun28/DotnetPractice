namespace Exercise04.Features.Audit
{
    public class ConfigurationAuditService
    {
        private readonly List<string> _logs = new();

        public void LogChange(string message)
        {
            _logs.Add($"{DateTime.UtcNow}: {message}");
        }
        public IEnumerable<string> GetLogs() => _logs;
    }
}