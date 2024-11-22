using Library.Application.AuthenticationUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RefreshAccessTokenRequestHandler : IRequestHandler<RefreshAccessTokenRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefreshAccessTokenRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> Handle(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                var refreshResponse = await _unitOfWork.UserRepository.RefreshAccessToken(request.Model);
                if(refreshResponse.User is null || refreshResponse.AccessToken == "")
                {
                    response.IsSuccess = false;
                    response.Message = "Access token not refreshed.";
                }
                else
                {
                    response.Result = refreshResponse;
                    response.IsSuccess = true;
                    response.Message = "Access token refreshed.";
                }


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while refreshing jwt token in:{ex}";
            }
            return response;
        }
    }
}
