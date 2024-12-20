using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksByGenreRequestHandler : IRequestHandler<GetBooksByGenreRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByGenreRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(GetBooksByGenreRequest request, CancellationToken cancellationToken)
        {
            var response=new ResponseData();
            try
            {
                response = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo,request.PageSize, cancellationToken,b => b.Genre == request.Genre);
                var books = ((ListModel<Book>)response.Result).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(books);
                response.Result = booksDtos;
                response.Message = $"Books with genre {request.Genre} are retrieved successfully.";
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
