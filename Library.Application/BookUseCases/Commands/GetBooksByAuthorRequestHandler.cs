using Library.Application.BookUseCases.Queries;
using Library.Application.Utilities;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksByAuthorRequestHandler : IRequestHandler<GetBooksByAuthorRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<BookDTO>>> Handle(GetBooksByAuthorRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<List<BookDTO>>();
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new CustomHttpException(400, "Bad request.", "Provide valid data for page number and page size.");
            }
            
            var bookList = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.AuthorId == request.AuthorId);
            var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
            response.Result = booksDtos;
            response.Message = $"Books are retrieved successfully.";
            response.IsSuccess = true;
            return response;
        }
    }
}
