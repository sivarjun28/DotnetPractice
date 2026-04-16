namespace Exercise02.Services
{
    public class CorrectSingleton1
    {
        private readonly IServiceProvider _serviceProvider;

        public CorrectSingleton1(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void DoWork()
        {
            using var scope = _serviceProvider.CreateScope();
            var requestContext = scope.ServiceProvider.GetRequiredService<IRequestContext>();

            Console.WriteLine("RequestId inside singleton: " + requestContext.RequestId);
        }
    }
}