namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record GetAllAuthorsRequest:IRequest<ResponseData<List<AuthorDTO>>>
    {
    }
}
