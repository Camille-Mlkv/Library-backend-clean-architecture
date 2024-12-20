namespace Library.Application.BookUseCases.Queries
{
    public sealed record UpdateBookRequest(int Id,BookDTO NewBook):IRequest<ResponseData>
    {
    }
}
