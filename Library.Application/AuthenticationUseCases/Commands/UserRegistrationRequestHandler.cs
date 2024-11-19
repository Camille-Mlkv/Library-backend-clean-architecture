using FluentValidation.Validators;
using Library.Application.AuthenticationUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserRegistrationRequestHandler : IRequestHandler<UserRegistrationRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRegistrationRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                string message = await _unitOfWork.UserRepository.Register(request.RegistrationRequest);
                if (!string.IsNullOrEmpty(message))
                {
                    response.IsSuccess = false;
                    response.Message = message;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "User with credentials was registered!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while registration:{ex}";
            }
            return response;
           
        }
    }
}
