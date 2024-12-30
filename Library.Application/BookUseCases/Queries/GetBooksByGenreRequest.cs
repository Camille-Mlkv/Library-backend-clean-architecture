namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksByGenreRequest(string Genre,int PageNo,int PageSize):IRequest<ResponseData<List<BookDTO>>>
    {
    }
}
