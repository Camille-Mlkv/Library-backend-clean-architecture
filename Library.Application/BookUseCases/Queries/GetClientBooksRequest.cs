namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetClientBooksRequest(string ClientId,string? CurrentUserId):IRequest<ResponseData>
    {
    }
}
