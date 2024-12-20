namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksPerPageRequest(int PageNo, int PageSize):IRequest<ResponseData>
    {
    }
}
