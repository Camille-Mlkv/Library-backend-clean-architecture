using Library.Application.AuthenticationUseCases.Queries;

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
            //mapping?
            User user = new()
            {
                UserName = request.RegistrationRequest.Email,
                Email = request.RegistrationRequest.Email,
                Name = request.RegistrationRequest.Name,
                PhoneNumber = request.RegistrationRequest.PhoneNumber
            };
            await _unitOfWork.UserRepository.CreateRole(request.RegistrationRequest.Role);

            var userCreated=await _unitOfWork.UserRepository.CreateUser(user,request.RegistrationRequest.Password);
            if (userCreated is not null) // id обязательно
            {
                await _unitOfWork.UserRepository.AddRoleToUser(userCreated, request.RegistrationRequest.Role);
                response.IsSuccess = true;
                response.Message = "Registrated successfully";
                response.Result = userCreated;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "An error occured while registrating the user";
            }
          
            return response;
           
        }
    }
}
