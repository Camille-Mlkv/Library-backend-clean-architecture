using Library.Application.AuthenticationUseCases.Queries;
using Library.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserLogInRequestHandler : IRequestHandler<UserLogInRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserLogInRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> Handle(UserLogInRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                var loginResponse = await _unitOfWork.UserRepository.Login(request.LoginRequest);
                if (loginResponse.User == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Credentials are incorrect.";
                }
                else
                {
                    response.Result = loginResponse;
                    response.IsSuccess = true;
                    response.Message = "Successfully logged in.";
                }
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while logging in:{ex}";
            }
            return response;
        }
    }
}
