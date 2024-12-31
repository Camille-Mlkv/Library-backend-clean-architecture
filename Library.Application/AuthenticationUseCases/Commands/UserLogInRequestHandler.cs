using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserLogInRequestHandler : IRequestHandler<UserLogInRequest, ResponseData<LoginResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public UserLogInRequestHandler(IUnitOfWork unitOfWork, ITokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ResponseData<LoginResponseDTO>> Handle(UserLogInRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<LoginResponseDTO>();

            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsername(request.LoginRequest.UserName);

                if (user == null || !await _unitOfWork.UserRepository.CheckPassword(user, request.LoginRequest.Password))
                {
                    response.Message = "Wrong credentials";
                    response.IsSuccess = false;
                    response.StatusCode = 401;
                    return response;
                }

                var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
                var (accessToken, expiry) = _jwtTokenGenerator.GenerateAccessToken(user, roles);

                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(24);

                await _unitOfWork.UserRepository.UpdateUser(user);
                await _unitOfWork.SaveAllAsync();


                LoginResponseDTO loginResponseDTO = new()
                {
                    User = user,
                    AccessToken = accessToken,
                    Expiration = expiry,
                    RefreshToken = refreshToken,
                };

                response.Result = loginResponseDTO;
                response.IsSuccess = true;
                response.Message = "Login successful";
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while logging in: {ex.Message}";
                response.StatusCode = 500;
            }

            return response;
        }
    }
}
