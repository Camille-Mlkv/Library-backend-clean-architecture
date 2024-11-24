using Library.Domain.Entities.Identity;
using Library.Infrastructure.Data;
using Library.Infrastructure.Identity;
using Library.Infrastructure.Identity.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,ITokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            if (user == null)
            {
                return new LoginResponseDTO() { User = null, AccessToken = "" };
            }
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (!isValid)
            {
                return new LoginResponseDTO() { User = null, AccessToken = "" };
            }

            //if user was found, generate access and refresh tokens
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user,roles);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // add refreshToken to database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(10);
            await _userManager.UpdateAsync(user);


            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDTO loginResponseDTO = new()
            {
                User = userDTO,
                AccessToken=new JwtSecurityTokenHandler().WriteToken(accessToken),
                Expiration=accessToken.ValidTo,
                RefreshToken = refreshToken,
            };
            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };
            // create role
            if (!_roleManager.RoleExistsAsync(registrationRequestDTO.Role).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(registrationRequestDTO.Role)).GetAwaiter().GetResult();
            }

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);
                    await _userManager.AddToRoleAsync(user, registrationRequestDTO.Role);

                    UserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";

                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }


        public async Task<LoginResponseDTO> RefreshAccessToken(RefreshModel refreshModel) // when access token is expired it is refreshed with refresh token
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(refreshModel.AccessToken);
            if (principal?.Identity?.Name is null)
                return new LoginResponseDTO() { User = null, AccessToken = "" };

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != refreshModel.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                return new LoginResponseDTO() { User = null, AccessToken = "" };

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDTO loginResponseDTO = new()
            {
                User = userDTO,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                Expiration = accessToken.ValidTo,
                RefreshToken = refreshModel.RefreshToken,
            };
            return loginResponseDTO;
        }

        public async Task<ResponseData> RevokeRefreshToken(string username)
        {
            var response=new ResponseData();
            var user = await _userManager.FindByNameAsync(username);
            if(user is null)
            {
                response.IsSuccess = false;
                response.Message = "User doesn't exist";
                return response;
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            response.IsSuccess = true;
            response.Message = "Refresh token revoked";
            return response;
        }

        public async Task<bool> UserExists(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return false;
            }
            return true;
        }



    }
    
}
