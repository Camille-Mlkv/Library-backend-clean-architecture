using Library.Domain.Entities.Identity;
using Library.Infrastructure.Data;
using Library.Infrastructure.Identity;
using Library.Infrastructure.Identity.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,IJwtTokenGenerator jwtTokenGenerator)
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
                AccessToken = accessToken,
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

    }
    
}
