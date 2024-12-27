namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBookImageRequest(int BookId): IRequest<byte[]>
    {
    }
}
