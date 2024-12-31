namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBookByIsbnRequest(string Isbn,int PageNo,int PageSize):IRequest<ResponseData<BookDTO>>
    {
    }
}
