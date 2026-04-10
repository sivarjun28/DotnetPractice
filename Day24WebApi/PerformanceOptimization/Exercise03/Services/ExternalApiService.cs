using Exercise03.Models;

namespace Exercise03.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;

        public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<ExternalProduct>> GetProductsAsync()
        {
            // TODO: Use HttpClient efficiently
            // - Reuse HttpClient instance (injected via DI)
            // - Use async methods
            // - Handle timeouts

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                var response = await _httpClient.GetAsync("/api/products", cts.Token);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<ExternalProduct>>() ?? new();
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Request timeout");
                throw;
            }
        }
    }

}