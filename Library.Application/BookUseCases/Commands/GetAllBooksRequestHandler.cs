using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetAllBooksRequestHandler : IRequestHandler<GetAllBooksRequest, ResponseData<List<BookDTO>>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetAllBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<List<BookDTO>>> Handle(GetAllBooksRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<List<BookDTO>>();
            var booksList = await _unitOfWork.BookRepository.ListAllAsync(cancellationToken);
            var booksDtos = _mapper.Map<List<BookDTO>>(booksList);

            responseData.Result = booksDtos;
            responseData.IsSuccess = true;
            responseData.Message = "All books are retrieved.";
            return responseData;
        }
    }
}
