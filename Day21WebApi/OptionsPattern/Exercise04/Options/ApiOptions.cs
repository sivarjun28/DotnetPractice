namespace Exercise04.Options
{
    public class ApiOptions
    {
        public const string Section = "Api";

        public string BaseUrl { get; set; } = string.Empty;
        public int RequestTimeoutSeconds { get; set; } = 30;
        public int MaxRetries { get; set; } = 3;
        public Dictionary<string, string> Headers { get; set; } = new();
    }
}