namespace Library.Application.BookUseCases.Queries
{
    public sealed record DeleteBookRequest(int Id):IRequest<ResponseData<object>>
    {
    }
}
