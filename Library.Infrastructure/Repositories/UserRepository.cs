using AutoMapper;
using Library.Infrastructure.Data;
using Library.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IMapper mapper) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<User> GetUserById(string userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            var user = _mapper.Map<User>(applicationUser);
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var applicationUser = await _userManager.FindByEmailAsync(username);
            return _mapper.Map<User>(applicationUser);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            var appUser=_mapper.Map<ApplicationUser>(user);
            bool isCorrect= await _userManager.CheckPasswordAsync(appUser, password);
            return isCorrect;
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            var appUser=_mapper.Map<ApplicationUser>(user);
            var roles= await _userManager.GetRolesAsync(appUser);
            return roles;
        }

        public async Task UpdateUser(User user)
        {
            var appUser=await _userManager.FindByIdAsync(user.Id);
            if (appUser is null) 
            {
                return;
            }

            appUser.RefreshToken = user.RefreshToken;
            appUser.RefreshTokenExpiry = user.RefreshTokenExpiry;

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
            var appUser = _mapper.Map<ApplicationUser>(user);
            appUser.Id = Guid.NewGuid().ToString();

            await _userManager.CreateAsync(appUser, password);
            return _mapper.Map<User>(appUser);
        }

        
        public async Task AddRoleToUser(User user, string role)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id);
            if (appUser is null)
            {
                return;
            }
            await _userManager.AddToRoleAsync(appUser, role);
            
        }
    }

}
