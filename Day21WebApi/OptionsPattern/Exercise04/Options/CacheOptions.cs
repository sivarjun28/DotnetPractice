namespace Exercise04.Options
{
    public class CacheOptions
    {
        public const string Section = "Cache";

        public bool Enabled { get; set; } = true;
        public int DefaultExpirationMinutes { get; set; } = 5;
        public int MaxSize { get; set; } = 1000;
        public string RedisConnection { get; set; } = string.Empty;
    }
}