using Library.Application.AuthenticationUseCases.Queries;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserRegistrationRequestHandler : IRequestHandler<UserRegistrationRequest, ResponseData<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRegistrationRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<User>> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<User>();
            try
            {
                User user = new()
                {
                    UserName = request.RegistrationRequest.Email,
                    Email = request.RegistrationRequest.Email,
                    Name = request.RegistrationRequest.Name,
                    PhoneNumber = request.RegistrationRequest.PhoneNumber
                };
                await _unitOfWork.UserRepository.CreateRole(request.RegistrationRequest.Role);

                var userCreated = await _unitOfWork.UserRepository.CreateUser(user, request.RegistrationRequest.Password);

                if (userCreated is null)
                {
                    response.IsSuccess = false;
                    response.Message = "An error occured while registrating the user";
                    response.StatusCode = 404;
                    return response;
                }

                await _unitOfWork.UserRepository.AddRoleToUser(userCreated, request.RegistrationRequest.Role);
                response.IsSuccess = true;
                response.Message = "Registrated successfully";
                response.Result = userCreated;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured during registration: {ex.Message}";
                response.StatusCode = 500;
            }
            
            return response;
           
        }
    }
}
