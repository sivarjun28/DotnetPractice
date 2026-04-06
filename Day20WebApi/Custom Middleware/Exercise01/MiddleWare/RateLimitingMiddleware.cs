namespace Execise01.MiddleWare
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly Dictionary<string, ClientRateLimit> _clientLimits = new();
        private static readonly SemaphoreSlim _lock = new(1, 1);
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);
        private static readonly int MaxRequestsPerWindow = 100;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string clientId = GetClientIdentifier(context);
            await _lock.WaitAsync();
            try
            {
                if (!_clientLimits.ContainsKey(clientId))
                {
                    _clientLimits[clientId] = new ClientRateLimit
                    {
                        RequestCount = 0,
                        WindowStart = DateTime.UtcNow,
                        LastRequest = DateTime.UtcNow
                    };
                }

                var client = _clientLimits[clientId];
                var now = DateTime.UtcNow;

                if (now - client.WindowStart > Window)
                {
                    client.RequestCount = 0;
                    client.WindowStart = now;
                }

                client.RequestCount++;
                client.LastRequest = now;

                if (client.RequestCount > MaxRequestsPerWindow)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = ((int)Window.TotalSeconds).ToString();
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }
            }
            finally
            {
                _lock.Release();
            }

            await _next(context);

            await CleanupOldEntries();
        }

        private string GetClientIdentifier(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
            {
                return apiKey.ToString();
            }
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private static async Task CleanupOldEntries()
        {
            await _lock.WaitAsync();
            try
            {
                var cutoff = DateTime.UtcNow - TimeSpan.FromMinutes(5);
                var keysToRemove = _clientLimits
                    .Where(kvp => kvp.Value.LastRequest < cutoff)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _clientLimits.Remove(key);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        private class ClientRateLimit
        {
            public int RequestCount { get; set; }
            public DateTime WindowStart { get; set; }
            public DateTime LastRequest { get; set; }
        }
    }
}