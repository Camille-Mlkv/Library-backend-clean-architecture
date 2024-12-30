using Library.Application.AuthenticationUseCases.Queries;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RevokeRefreshTokenRequestHandler : IRequestHandler<RevokeRefreshTokenRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeRefreshTokenRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<object>> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<object>();
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsername(request.Username);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = "User doesn't exist";
                    response.StatusCode = 404;
                    return response;
                }
                user.RefreshToken = null;
                await _unitOfWork.UserRepository.UpdateUser(user);

                response.IsSuccess = true;
                response.Message = "Refresh token revoked";
                response.StatusCode = 204;
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while revoking refresh token {ex.Message}";
                response.StatusCode = 500;
            }
            return response;
        }
    }
}
