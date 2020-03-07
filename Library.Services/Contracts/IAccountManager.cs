using Library.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services.Contracts
{
    public interface IAccountManager
    {
        Task<User> CreateAsync(string username, string password, int membershipMonths);
        User Find(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(string id);
        Task<User> ActivateUserAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task BanUserAsync(string id);
        void CheckStatus(User user);
        Task<User> GetAdminAccountAsync();
    }
}
