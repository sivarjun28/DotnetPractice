namespace Exercise01.MiddleWare
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private const string CorrelationIdItemKey = "CorrelationId";

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            

            string correlationId;
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var headerValue) &&
                !string.IsNullOrWhiteSpace(headerValue))
            {
                correlationId = headerValue!;
            }
            else
            {
               
                correlationId = Guid.NewGuid().ToString();
            }

           
            context.Items[CorrelationIdItemKey] = correlationId;
           
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                {
                    context.Response.Headers.Add(CorrelationIdHeader, correlationId);
                }
                return Task.CompletedTask;
            });
            
            using (_logger.BeginScope(new System.Collections.Generic.Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            
            {
                _logger.LogInformation("Handling request with CorrelationId: {CorrelationId}", correlationId);

               
                await _next(context);

               
                _logger.LogInformation("Finished handling request with CorrelationId: {CorrelationId}", correlationId);
            }

        }
    }
}