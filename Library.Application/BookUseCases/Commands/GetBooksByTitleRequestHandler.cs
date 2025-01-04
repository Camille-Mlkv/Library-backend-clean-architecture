using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksByTitleRequestHandler : IRequestHandler<GetBooksByTitleRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByTitleRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<BookDTO>>> Handle(GetBooksByTitleRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<List<BookDTO>>();
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var bookList = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.Title == request.Title);
            var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
            response.Result = booksDtos;
            response.Message = $"Books are retrieved successfully.";
            response.IsSuccess = true;
            return response;
        }
    }
}
