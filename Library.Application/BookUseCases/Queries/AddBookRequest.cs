namespace Library.Application.BookUseCases.Queries
{
    public sealed record AddBookRequest(BookDTO book):IRequest<ResponseData>
    {
    }
}
