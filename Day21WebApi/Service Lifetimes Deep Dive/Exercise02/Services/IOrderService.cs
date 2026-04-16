using Exercise02.Services;
namespace Exercise02.Services
{

    public interface IOrderService
    {
        Task ProcessPendingOrdersAsync(IDbContext dbContext, CancellationToken cancellationToken);
    }

    public class OrderServiceStub : IOrderService
    {
        private readonly ILogger<OrderServiceStub> _logger;

        public OrderServiceStub(ILogger<OrderServiceStub> logger)
        {
            _logger = logger;
        }

        public async Task ProcessPendingOrdersAsync(IDbContext dbContext, CancellationToken cancellationToken)
        {
            var pendingOrders = await dbContext.GetPendingOrdersAsync();

            foreach (var order in pendingOrders)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                
                _logger.LogInformation("Processing order {Id}: {Description}", order.Id, order.Description);
                await Task.Delay(500, cancellationToken); 
                await dbContext.MarkOrderProcessedAsync(order);
            }
        }
    }
}