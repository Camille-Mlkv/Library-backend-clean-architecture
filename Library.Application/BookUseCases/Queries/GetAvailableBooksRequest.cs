namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetAvailableBooksRequest(int PageNo, int PageSize):IRequest<ResponseData<List<BookDTO>>>
    {
    }
}
