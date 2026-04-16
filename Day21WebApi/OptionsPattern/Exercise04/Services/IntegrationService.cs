using Exercise04.Options;
using Microsoft.Extensions.Options;

namespace Exercise04.Services
{
    public class IntegrationService
    {
        private readonly ApiClientOptions _orders;
        private readonly ApiClientOptions _payments;

        public IntegrationService(IOptionsSnapshot<ApiClientOptions> options)
        {
            _orders = options.Get("OrdersApi");
            _payments = options.Get("PaymentsApi");
        }
    }
}