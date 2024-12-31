using Library.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> UserExists(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return false;
            }
            return true;
        }

        public async Task<User> GetUserById(string userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null)
            {
                return null;
            }

            //mapping
            return new User
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                Name = applicationUser.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                RefreshToken = applicationUser.RefreshToken,
                RefreshTokenExpiry = applicationUser.RefreshTokenExpiry,
            };
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var applicationUser = await _userManager.FindByEmailAsync(username);

            if (applicationUser == null)
            {
                return null;
            }

            //mapping
            return new User
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                Name = applicationUser.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                RefreshToken = applicationUser.RefreshToken,
                RefreshTokenExpiry = applicationUser.RefreshTokenExpiry,
            };
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            // mapping
            ApplicationUser appUser = new()
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
            };

            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            //mapping
            ApplicationUser appUser = new()
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
            };

            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task UpdateUser(User user)
        {
            ApplicationUser appUser = new()
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
            };

            appUser.RefreshToken = user.RefreshToken;
            appUser.RefreshTokenExpiry= user.RefreshTokenExpiry;
            await _userManager.UpdateAsync(appUser);
            
        }

        public async Task CreateRole(string role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        public async Task<bool> RoleExists(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<User> CreateUser(User user, string password)
        {
            //mapping
            ApplicationUser appUser = new()
            {
                UserName = user.Email,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
            };
            
            var result = await _userManager.CreateAsync(appUser, password);
            User userToReturn = new()
            {
                Id=appUser.Id,
                UserName=appUser.UserName,
                Email=appUser.Email,
                PhoneNumber=appUser.PhoneNumber,
                Name=appUser.Name,
                RefreshToken=appUser.RefreshToken,
                RefreshTokenExpiry=appUser.RefreshTokenExpiry,
            };
            return userToReturn;
        }

        
        public async Task AddRoleToUser(User user, string role)
        {
            //mapping
            ApplicationUser appUser = new()
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
            };

            await _userManager.AddToRoleAsync(appUser, role);
            
            
        }
    }

}
