namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record DeleteAuthorRequest(int id):IRequest<ResponseData>
    {
    }
}
