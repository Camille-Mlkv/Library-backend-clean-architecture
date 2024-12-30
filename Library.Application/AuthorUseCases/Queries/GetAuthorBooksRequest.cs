namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record GetAuthorBooksRequest(int id):IRequest<ResponseData<Author>>
    {
    }
}
