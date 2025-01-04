using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;
using Library.Application.Exceptions;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RefreshAccessTokenRequestHandler : IRequestHandler<RefreshAccessTokenRequest, ResponseData<LoginResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public RefreshAccessTokenRequestHandler(IUnitOfWork unitOfWork,ITokenGenerator tokenGenerator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = tokenGenerator;
            _mapper = mapper;
        }
        public async Task<ResponseData<LoginResponseDTO>> Handle(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<LoginResponseDTO>();
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.RefreshModel.AccessToken);
            if (principal?.Identity?.Name is null)
            {
                throw new BadRequestException("Access token not refreshed.", "Old access token is invalid.");
            }

            var user = await _unitOfWork.UserRepository.GetUserByUsername(principal.Identity.Name);
            if (user is null || user.RefreshToken != request.RefreshModel.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new BadRequestException("Access token not refreshed.", "Refresh token expired or is invalid.");
            }
            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);

            LoginResponseDTO loginResponseDTO = new()
            {
                User = _mapper.Map<UserDTO>(user),
                AccessToken = accessToken.AccessToken,
                Expiration = accessToken.Expiry,
                RefreshToken = request.RefreshModel.RefreshToken,
            };

            response.Result = loginResponseDTO;
            response.IsSuccess = true;
            response.Message = "Access token refreshed";
            return response;
        }
    }
}
