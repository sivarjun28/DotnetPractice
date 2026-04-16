namespace Exercise02.Services
{
    public class OrderProcessingBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderProcessingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public OrderProcessingBackgroundService(
            ILogger<OrderProcessingBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

                try
                {
                    await orderService.ProcessPendingOrdersAsync(dbContext, stoppingToken);
                    _logger.LogInformation("Processed orders at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing orders");
                }

                
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}