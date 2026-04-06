namespace Exercise04.Models
{
    public class ApiMetadata
    {
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0";
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
    }
}