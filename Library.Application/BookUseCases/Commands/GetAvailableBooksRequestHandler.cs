using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetAvailableBooksRequestHandler : IRequestHandler<GetAvailableBooksRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<BookDTO>>> Handle(GetAvailableBooksRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<List<BookDTO>>();
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }
            
            var bookList = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.ClientId == null);
            var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
            response.Result = booksDtos;
            response.Message = "Available books retrieved successfully.";
            response.IsSuccess = true;
            return response;
        }
    }
}
