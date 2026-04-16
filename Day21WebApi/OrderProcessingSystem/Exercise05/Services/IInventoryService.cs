namespace Exercise05.Services
{
    public interface IInventoryService
    {
        Task<bool> CheckAvailabilityAsync(int productId, int quantity);
        Task ReserveAsync(int productId, int quantity);
        Task ReleaseAsync(int productId, int quantity);
    }

}