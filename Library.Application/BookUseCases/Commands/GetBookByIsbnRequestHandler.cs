using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBookByIsbnRequestHandler : IRequestHandler<GetBookByIsbnRequest, ResponseData<BookDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookByIsbnRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<BookDTO>> Handle(GetBookByIsbnRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<BookDTO>();
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                response.IsSuccess = false;
                response.Message = "Provide valid data for page number and size";
                response.StatusCode = 400;
                return response;
            }
            try
            {
                var book =(await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.ISBN == request.Isbn)).First();
                var bookDto = _mapper.Map<BookDTO>(book);
                response.Result = bookDto;
                response.Message = $"Book with ISBN {request.Isbn} retrieved successfully.";
                response.IsSuccess = true;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while retrieving books:{ex}";
                response.StatusCode = 500;
            }

            return response;
        }
    }
}
