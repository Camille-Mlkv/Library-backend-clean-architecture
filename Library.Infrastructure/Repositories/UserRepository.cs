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

        //public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        //{
        //var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
        //if (user == null)
        //{
        //    return new LoginResponseDTO() { User = null, AccessToken = "" };
        //}
        //bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        //if (!isValid)
        //{
        //    return new LoginResponseDTO() { User = null, AccessToken = "" };
        //}

        ////if user was found, generate access and refresh tokens
        //var roles = await _userManager.GetRolesAsync(user);
        //var accessToken = _jwtTokenGenerator.GenerateAccessToken(user,roles);
        //var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        //// add refreshToken to database
        //user.RefreshToken = refreshToken;
        //user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(10); // 10 hours for refresh token
        //await _userManager.UpdateAsync(user);


        //UserDTO userDTO = new()
        //{
        //    Email = user.Email,
        //    Id = user.Id,
        //    Name = user.Name,
        //    PhoneNumber = user.PhoneNumber,
        //};

        //LoginResponseDTO loginResponseDTO = new()
        //{
        //    User = userDTO,
        //    AccessToken=new JwtSecurityTokenHandler().WriteToken(accessToken),
        //    Expiration=accessToken.ValidTo,
        //    RefreshToken = refreshToken,
        //};
        //LoginResponseDTO loginResponseDTO = new();
        //return loginResponseDTO;
        //}

        //public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        //{
        //ApplicationUser user = new()
        //{
        //    UserName = registrationRequestDTO.Email,
        //    Email = registrationRequestDTO.Email,
        //    NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
        //    Name = registrationRequestDTO.Name,
        //    PhoneNumber = registrationRequestDTO.PhoneNumber
        //};
        //// create role
        //if (!_roleManager.RoleExistsAsync(registrationRequestDTO.Role).GetAwaiter().GetResult())
        //{
        //    _roleManager.CreateAsync(new IdentityRole(registrationRequestDTO.Role)).GetAwaiter().GetResult();
        //}

        //try
        //{
        //    var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
        //    if (result.Succeeded)
        //    {
        //        var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);
        //        await _userManager.AddToRoleAsync(user, registrationRequestDTO.Role);

        //        UserDTO userDto = new()
        //        {
        //            Email = userToReturn.Email,
        //            Id = userToReturn.Id,
        //            Name = userToReturn.Name,
        //            PhoneNumber = userToReturn.PhoneNumber
        //        };

        //        return "";

        //    }
        //    else
        //    {
        //        return result.Errors.FirstOrDefault().Description;
        //    }

        //}
        //catch (Exception ex)
        //{

        //}
        //return "Error Encountered";
        //}


        //public async Task<LoginResponseDTO> RefreshAccessToken(RefreshModel refreshModel) // when access token is expired it is refreshed with refresh token
        //{
        //var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(refreshModel.AccessToken);
        //if (principal?.Identity?.Name is null)
        //    return new LoginResponseDTO() { User = null, AccessToken = "" };

        //var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        //if (user is null || user.RefreshToken != refreshModel.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        //    return new LoginResponseDTO() { User = null, AccessToken = "" };

        //var roles = await _userManager.GetRolesAsync(user);
        //var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);

        //UserDTO userDTO = new()
        //{
        //    Email = user.Email,
        //    Id = user.Id,
        //    Name = user.Name,
        //    PhoneNumber = user.PhoneNumber,
        //};

        //LoginResponseDTO loginResponseDTO = new();
        //{
        //    User = userDTO,
        //    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
        //    Expiration = accessToken.ValidTo,
        //    RefreshToken = refreshModel.RefreshToken,
        //};
        //return loginResponseDTO;
        //}

        // public async Task<ResponseData> RevokeRefreshToken(string username)
        //{
        //var response=new ResponseData();
        //var user = await _userManager.FindByNameAsync(username);
        //if(user is null)
        //{
        //    response.IsSuccess = false;
        //    response.Message = "User doesn't exist";
        //    return response;
        //}

        //user.RefreshToken = null;
        //await _userManager.UpdateAsync(user);

        //response.IsSuccess = true;
        //response.Message = "Refresh token revoked";
        //return response;
        //}

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

        public async Task<bool> CheckPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if(user is null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id);
            return appUser != null ? await _userManager.GetRolesAsync(appUser) : Enumerable.Empty<string>();
        }

        public async Task UpdateUser(User user)
        {
            var appUser=await _userManager.FindByIdAsync(user.Id);
            if(appUser != null)
            {
                appUser.RefreshToken = user.RefreshToken;
                appUser.RefreshTokenExpiry= user.RefreshTokenExpiry;
                await _userManager.UpdateAsync(appUser);
            }
            
        }

        public async Task CreateRole(string role)
        {
            bool roleExists=await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
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
            var appUser = await _userManager.FindByIdAsync(user.Id);
            if(appUser != null)
            {
                await _userManager.AddToRoleAsync(appUser, role);
            }
            
        }
    }

}
