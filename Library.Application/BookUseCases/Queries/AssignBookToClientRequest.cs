namespace Library.Application.BookUseCases.Queries
{
    public sealed record AssignBookToClientRequest(int BookId, string ClientId):IRequest<ResponseData>
    {
    }
}
