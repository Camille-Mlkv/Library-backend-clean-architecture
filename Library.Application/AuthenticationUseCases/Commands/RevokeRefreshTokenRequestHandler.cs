using Library.Application.AuthenticationUseCases.Queries;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RevokeRefreshTokenRequestHandler : IRequestHandler<RevokeRefreshTokenRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeRefreshTokenRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            var user = await _unitOfWork.UserRepository.GetUserByUsername(request.Username);
            if(user is null)
            {
                response.IsSuccess = false;
                response.Message = "User doesn't exist";
                return response;
            }
            user.RefreshToken = null;
            await _unitOfWork.UserRepository.UpdateUser(user);

            response.IsSuccess = true;
            response.Message = "Refresh token revoked";
            //try
            //{
            //    response = await _unitOfWork.UserRepository.RevokeRefreshToken(request.Username);
            //    return response;
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
