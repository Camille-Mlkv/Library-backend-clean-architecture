using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksByIsbnRequestHandler : IRequestHandler<GetBooksByIsbnRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByIsbnRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(GetBooksByIsbnRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                response = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.ISBN == request.Isbn);
                var books = ((ListModel<Book>)response.Result).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(books);
                response.Result = booksDtos;
                response.Message = $"Books with ISBN {request.Isbn} are retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while retrieving books:{ex}";
            }

            return response;
        }
    }
}
