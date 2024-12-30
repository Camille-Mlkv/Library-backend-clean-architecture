namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksByIsbnRequest(string Isbn,int PageNo,int PageSize):IRequest<ResponseData<List<BookDTO>>>
    {
    }
}
