using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;
using Library.Application.Exceptions;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class UserRegistrationRequestHandler : IRequestHandler<UserRegistrationRequest, ResponseData<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRegistrationRequestHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<UserDTO>> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<UserDTO>();
            User user = new()
            {
                UserName = request.RegistrationRequest.Email,
                Email = request.RegistrationRequest.Email,
                Name = request.RegistrationRequest.Name,
                PhoneNumber = request.RegistrationRequest.PhoneNumber
            };
            if(!(await _unitOfWork.UserRepository.RoleExists(request.RegistrationRequest.Role)))
            {
                await _unitOfWork.UserRepository.CreateRole(request.RegistrationRequest.Role);
            }
                
            var userCreated = await _unitOfWork.UserRepository.CreateUser(user, request.RegistrationRequest.Password);

            if (userCreated is null)
            {
                throw new BadRequestException("User not created.");
            }

            await _unitOfWork.UserRepository.AddRoleToUser(userCreated, request.RegistrationRequest.Role);

            var userDto=_mapper.Map<UserDTO>(userCreated);
            response.IsSuccess = true;
            response.Message = "Registrated successfully";
            response.Result = userDto;
            return response;
           
        }
    }
}
