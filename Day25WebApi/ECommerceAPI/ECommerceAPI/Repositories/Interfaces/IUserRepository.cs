using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repositories.Interfaces
{

    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task<bool> UserExistsAsync(string username);
    }
}
