using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;


namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RefreshAccessTokenRequestHandler : IRequestHandler<RefreshAccessTokenRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public RefreshAccessTokenRequestHandler(IUnitOfWork unitOfWork,ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = tokenGenerator;
        }
        public async Task<ResponseData> Handle(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();

            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.RefreshModel.AccessToken);
            if (principal?.Identity?.Name is null)
            {
                response.Message = "Access token not refreshed";
                response.IsSuccess = false;
                return response;
            }

            var user = await _unitOfWork.UserRepository.GetUserByUsername(principal.Identity.Name);
            if (user is null || user.RefreshToken != request.RefreshModel.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                response.Message = "Access token not refreshed";
                response.IsSuccess = false;
                return response;
            }
            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);

            LoginResponseDTO loginResponseDTO = new()
            {
                User=user,
                AccessToken = accessToken.AccessToken,
                Expiration = accessToken.Expiration,
                RefreshToken = request.RefreshModel.RefreshToken,
            };

            response.Result = loginResponseDTO;
            response.IsSuccess = true;
            response.Message = "Access token refreshed";
            //try
            //{
            //    var refreshResponse = await _unitOfWork.UserRepository.RefreshAccessToken(request.RefreshModel);
            //    if(refreshResponse.User is null || refreshResponse.AccessToken == "")
            //    {
            //        response.IsSuccess = false;
            //        response.Message = "Access token not refreshed.";
            //    }
            //    else
            //    {
            //        response.Result = refreshResponse;
            //        response.IsSuccess = true;
            //        response.Message = "Access token refreshed.";
            //    }


            //}
            //catch (Exception ex)
            //{
            //    response.IsSuccess = false;
            //    response.Message = $"An error occured while refreshing jwt token in:{ex}";
            //}
            return response;
        }
    }
}
