using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                response = await _unitOfWork.UserRepository.RevokeRefreshToken(request.Username);
                return response;
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
