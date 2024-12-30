namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record UpdateAuthorRequest(int id, AuthorDTO author):IRequest<ResponseData<AuthorDTO>>
    {
    }
}
