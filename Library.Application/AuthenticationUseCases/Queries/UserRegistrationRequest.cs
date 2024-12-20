using Library.Application.DTOs.Identity;

namespace Library.Application.AuthenticationUseCases.Queries
{
    public sealed record UserRegistrationRequest(RegistrationRequestDTO RegistrationRequest):IRequest<ResponseData>
    {
    }
}
