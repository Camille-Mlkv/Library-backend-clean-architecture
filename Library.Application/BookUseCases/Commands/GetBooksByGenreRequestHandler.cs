using Library.Application.BookUseCases.Queries;
using Library.Domain.Entities;

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
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                response.IsSuccess = false;
                response.Message = "Provide valid data for page number and size";
                return response;
            }
            try
            {
                var bookList = (await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.Genre == request.Genre)).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
                response.Result = booksDtos;
                response.Message = $"Books with genre {request.Genre} are retrieved successfully.";
                response.IsSuccess = true;
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
