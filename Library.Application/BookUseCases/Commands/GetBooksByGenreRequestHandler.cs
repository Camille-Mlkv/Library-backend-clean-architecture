using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksByGenreRequestHandler : IRequestHandler<GetBooksByGenreRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByGenreRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<BookDTO>>> Handle(GetBooksByGenreRequest request, CancellationToken cancellationToken)
        {
            var response=new ResponseData<List<BookDTO>>();
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var bookList = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.Genre == request.Genre);
            var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
            response.Result = booksDtos;
            response.Message = $"Books with genre {request.Genre} are retrieved successfully.";
            response.IsSuccess = true;
            return response;
        }
    }
}
