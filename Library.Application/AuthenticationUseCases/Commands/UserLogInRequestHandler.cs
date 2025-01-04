using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;
using Library.Application.Exceptions;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserLogInRequestHandler : IRequestHandler<UserLogInRequest, ResponseData<LoginResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public UserLogInRequestHandler(IUnitOfWork unitOfWork, ITokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<ResponseData<LoginResponseDTO>> Handle(UserLogInRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<LoginResponseDTO>();
            var user = await _unitOfWork.UserRepository.GetUserByUsername(request.LoginRequest.UserName);

            if (user == null || !await _unitOfWork.UserRepository.CheckPassword(user, request.LoginRequest.Password))
            {
                throw new UnauthorizedException("Wrong credentials");
            }

            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            var (accessToken, expiry) = _jwtTokenGenerator.GenerateAccessToken(user, roles);

            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _unitOfWork.UserRepository.UpdateUserTokens(user);
            await _unitOfWork.SaveAllAsync();

            LoginResponseDTO loginResponseDTO = new()
            {
                User = _mapper.Map<UserDTO>(user),
                AccessToken = accessToken,
                Expiration = expiry,
                RefreshToken = refreshToken,
            };

            response.Result = loginResponseDTO;
            response.IsSuccess = true;
            response.Message = "Login successful";
            return response;
        }
    }
}
