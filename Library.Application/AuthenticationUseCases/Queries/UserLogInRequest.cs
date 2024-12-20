using Library.Application.DTOs.Identity;

namespace Library.Application.AuthenticationUseCases.Queries
{
    public sealed record UserLogInRequest(LoginRequestDTO LoginRequest):IRequest<ResponseData>
    {
    }
}
