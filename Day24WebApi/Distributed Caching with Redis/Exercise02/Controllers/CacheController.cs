using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Exercise02.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;

        public CacheController(
            IDistributedCache cache,
            IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;
        }

        [HttpGet("stats")]
        public ActionResult GetStats()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var db = _redis.GetDatabase();

            var endpoints = _redis.GetEndPoints();
            var stats = new List<object>();

            foreach (var endpoint in endpoints)
            {
                var srv = _redis.GetServer(endpoint);

                stats.Add(new
                {
                    Endpoint = endpoint.ToString(),
                    DatabaseSize = srv.DatabaseSize(),
                    IsConnected = srv.IsConnected
                });
            }

            return Ok(stats);
        }

        [HttpPost("clear")]
        public async Task<ActionResult> ClearCache()
        {
            var endpoints = _redis.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                await server.FlushDatabaseAsync();
            }

            return Ok("Cache cleared successfully");
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult> RemoveKey(string key)
        {
            await _cache.RemoveAsync(key);
            return NoContent();
        }
    }
}