namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBookByIdRequest(int id):IRequest<ResponseData>
    {
    }
}
