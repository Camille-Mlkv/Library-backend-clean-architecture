namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksByTitleRequest(string Title, int PageNo, int PageSize):IRequest<ResponseData<List<BookDTO>>>
    {
    }
}
