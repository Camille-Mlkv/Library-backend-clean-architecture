using Library.Domain.Entities;

namespace Library.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string userId);
        Task<User> GetUserByUsername(string username);
        Task<bool> CheckPassword(User user, string password);
        Task<IEnumerable<string>> GetUserRoles(User user);
        Task UpdateUserTokens(User user);
        Task CreateRole(string role);
        Task<bool> RoleExists(string role);
        Task<User> CreateUser(User user,string password);
        Task AddRoleToUser(User user,string role);
    }
}
