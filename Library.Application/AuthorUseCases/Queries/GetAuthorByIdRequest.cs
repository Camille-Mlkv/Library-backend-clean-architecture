namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record GetAuthorByIdRequest(int  id):IRequest<ResponseData>
    {
    }
}
