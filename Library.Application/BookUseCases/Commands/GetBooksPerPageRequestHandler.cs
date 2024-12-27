using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksPerPageRequestHandler : IRequestHandler<GetBooksPerPageRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksPerPageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(GetBooksPerPageRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            if(request.PageNo<1 || request.PageSize < 1)
            {
                responseData.IsSuccess = false;
                responseData.Message = "Provide valid data for page number and size";
                return responseData;
            }
            try
            {
                var bookList=(await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken)).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
                responseData.Result = booksDtos;
                responseData.Message = "Books retrieved successfully.";
                responseData.IsSuccess=true;
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error: {ex.Message}";
            }
            return responseData;
        }
    }
}
