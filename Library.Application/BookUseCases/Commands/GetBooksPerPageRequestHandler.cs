using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksPerPageRequestHandler : IRequestHandler<GetBooksPerPageRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksPerPageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<List<BookDTO>>> Handle(GetBooksPerPageRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<List<BookDTO>>();
            if(request.PageNo<1 || request.PageSize < 1)
            {
                responseData.IsSuccess = false;
                responseData.Message = "Provide valid data for page number and size";
                responseData.StatusCode = 400;
                return responseData;
            }
            try
            {
                var bookList=(await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken)).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
                responseData.Result = booksDtos;
                responseData.Message = "Books retrieved successfully.";
                responseData.IsSuccess=true;
                responseData.StatusCode = 200;
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error: {ex.Message}";
                responseData.StatusCode = 500;
            }
            return responseData;
        }
    }
}
