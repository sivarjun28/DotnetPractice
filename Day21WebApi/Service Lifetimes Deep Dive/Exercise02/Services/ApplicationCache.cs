namespace Exercise02.Services
{
    public class ApplicationCache : IApplicationCache
    {

        private readonly Dictionary<string, CacheEntry> _cache = new();
        private readonly object _lock = new();
        public object? Get(string key)
        {
            lock (_lock)
            {
                if(_cache.TryGetValue(key, out var entry))
                {
                    return entry.Value;
                }
            }
            return null;
        }

        public void Set(string key, object value)
        {
           var entry = new CacheEntry
           {
               Value = value,
               CreatedAt = DateTime.UtcNow
           };
            lock (_lock)
            {
                _cache[key] = entry;
            }
        }

        private class CacheEntry
        {
            public object Value { get; set; } = null!;
            public DateTime CreatedAt { get; set; }
        }
    }
    
}