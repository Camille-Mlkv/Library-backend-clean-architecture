using Library.Application.AuthorUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAuthorBooksRequestHandler : IRequestHandler<GetAuthorBooksRequest, ResponseData<Author>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<Author>> Handle(GetAuthorBooksRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<Author>();
            var authorWithBooks=await _unitOfWork.AuthorRepository.ListAsync(a => a.Id == request.id, cancellationToken, a => a.Books);
            if(authorWithBooks is null)
            {
                throw new NotFoundException($"Author with id {request.id} doesn't exist.");
            }
            responseData.Result = authorWithBooks.First();
            responseData.IsSuccess = true;
            responseData.Message = "Books found for specified author.";
            return responseData;
        }
    }
}
