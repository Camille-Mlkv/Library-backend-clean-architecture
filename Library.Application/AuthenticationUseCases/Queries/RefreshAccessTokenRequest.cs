using Library.Application.DTOs.Identity;

namespace Library.Application.AuthenticationUseCases.Queries
{
    public sealed record RefreshAccessTokenRequest(RefreshModelDTO RefreshModel):IRequest<ResponseData<LoginResponseDTO>>
    {
    }
}
