namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetClientBooksRequest(string ClientId):IRequest<ResponseData>
    {
    }
}
