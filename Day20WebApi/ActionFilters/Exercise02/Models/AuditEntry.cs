namespace Exercise02.Models
{
    public class AuditEntry
    {
        public DateTime Timestamp { get; set; }
        public string User { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public Dictionary<string, object?> Parameters { get; set; } = new();
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string? Exception { get; set; }
    }
}