using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Exercise02.Services
{
    public class RedisDistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisDistributedCacheService> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisDistributedCacheService(
            IDistributedCache cache,
            ILogger<RedisDistributedCacheService> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = cache;
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _cache.GetStringAsync(key, cancellationToken);

                if (string.IsNullOrEmpty(data))
                {
                    _logger.LogDebug($"Cache miss for key: {key}");
                    return default;
                }

                _logger.LogDebug($"Cache hit for key: {key}");
                return JsonSerializer.Deserialize<T>(data, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting cache key: {key}");
                return default;
            }
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();

                if (expiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = expiration;
                }
                else
                {
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                }

                var json = JsonSerializer.Serialize(value, _jsonOptions);
                await _cache.SetStringAsync(key, json, options, cancellationToken);

                _logger.LogDebug($"Cached key: {key}, expires in: {options.AbsoluteExpirationRelativeToNow}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error setting cache key: {key}");
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
                _logger.LogDebug($"Removed cache key: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing cache key: {key}");
            }
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            var data = await _cache.GetStringAsync(key, cancellationToken);
            return !string.IsNullOrEmpty(data);
        }

        public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern);
            _logger.LogWarning($"RemoveByPatternAsync not fully implemented for pattern: {pattern}");
            foreach (var key in keys)
            {
                await _cache.RemoveAsync(key);
            }
        }
    }
}