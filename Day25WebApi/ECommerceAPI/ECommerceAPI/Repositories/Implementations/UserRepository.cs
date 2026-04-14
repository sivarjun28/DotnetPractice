using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ECommerceAPI.Repositories.Implementations
{
    
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public async Task CreateUserAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await Task.FromResult(_users.Any(u => u.Username == username));
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await Task.FromResult(_users.Any(u => u.Email == email));
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.Username = user.Username;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(User user)
        {
            _users.Remove(user);
            await Task.CompletedTask;
        }
    }
}