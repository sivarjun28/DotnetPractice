using Exercise01.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Exercise01.Repository
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheService> _logger;
        private readonly HashSet<string> _keys = new();

        public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);

            _logger.LogInformation($"Cache get: {key}");

            return await Task.FromResult(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            _cache.Set(key, value, options);
            _keys.Add(key);

            _logger.LogInformation($"Cache set: {key}");

            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);

            _logger.LogInformation($"Cache removed: {key}");

            await Task.CompletedTask;
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var keysToRemove = _keys.Where(k => k.StartsWith(prefix)).ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _keys.Remove(key);
            }

            _logger.LogInformation($"Cache cleared by prefix: {prefix}");

            await Task.CompletedTask;
        }
    }
}