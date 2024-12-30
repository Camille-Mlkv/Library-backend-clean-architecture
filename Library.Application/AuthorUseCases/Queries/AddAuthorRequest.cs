namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record AddAuthorRequest(AuthorDTO authorDto):IRequest<ResponseData<AuthorDTO>>
    {
    }
}
