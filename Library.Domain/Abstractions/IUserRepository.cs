using Library.Domain.Entities;

namespace Library.Domain.Abstractions
{
    public interface IUserRepository
    {
        //Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        //Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        //Task<LoginResponseDTO> RefreshAccessToken(RefreshModel refreshModel);

        //Task<ResponseData> RevokeRefreshToken(string username);

        Task<bool> UserExists(string userId); // get rid of it
        Task<User> GetUserById(string userId);
        Task<User> GetUserByUsername(string username);
        Task<bool> CheckPassword(string username, string password);
        Task<IEnumerable<string>> GetUserRoles(User user);
        Task UpdateUser(User user);
        Task CreateRole(string role);
        Task<User> CreateUser(User user,string password);
        Task AddRoleToUser(User user,string role);
    }
}
