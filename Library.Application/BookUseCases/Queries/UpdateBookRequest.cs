namespace Library.Application.BookUseCases.Queries
{
    public sealed record UpdateBookRequest(int Id,BookDTO UpdatedBook):IRequest<ResponseData<object>>
    {
    }
}
