namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksByAuthorRequest(int AuthorId,int PageNo,int PageSize):IRequest<ResponseData>
    {
    }
}
