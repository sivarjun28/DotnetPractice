namespace Exercise02.Services
{
    public class CorrectSingleton2
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CorrectSingleton2(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void DoWork()
        {
            using var scope = _scopeFactory.CreateScope();
            var requestContext = scope.ServiceProvider.GetRequiredService<IRequestContext>();

            Console.WriteLine("RequestId inside singleton: " + requestContext.RequestId);
        }
    }
}