namespace Exercise05.Services
{
    public class InventoryService : IInventoryService
    {

        private readonly Dictionary<int, int> _stock = new();
        public InventoryService()
        {
            _stock[1] = 100;
            _stock[2] = 50;


        }
        public Task<bool> CheckAvailabilityAsync(int productId, int quantity)
        {
           bool available = _stock.ContainsKey(productId) && _stock[productId] >= quantity;
           return Task.FromResult(available);
        }

        public Task ReleaseAsync(int productId, int quantity)
        {
            if(_stock.ContainsKey(productId))
                _stock[productId] += quantity;
            return Task.CompletedTask;
        }

        public Task ReserveAsync(int productId, int quantity)
        {
            if(!_stock.ContainsKey(productId) || _stock[productId] < quantity){
                throw new InvalidOperationException("Not enough stock");
            }
            _stock[productId] -= quantity;
            return Task.CompletedTask;
        }
    }
}