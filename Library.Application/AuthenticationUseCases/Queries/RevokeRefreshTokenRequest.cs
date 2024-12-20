namespace Library.Application.AuthenticationUseCases.Queries
{
    public sealed record RevokeRefreshTokenRequest(string Username):IRequest<ResponseData>
    {
    }
}
