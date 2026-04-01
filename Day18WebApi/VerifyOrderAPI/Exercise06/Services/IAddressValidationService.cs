namespace Exercise06.Services
{
    public interface IAddressValidationService
    {
        Task<bool> ValidateAsync(string address);
    }
    public class AddressValidationService : IAddressValidationService
    {
        public Task<bool> ValidateAsync(string address)
        {
           
            return Task.FromResult(!string.IsNullOrWhiteSpace(address));
        }
    }
}