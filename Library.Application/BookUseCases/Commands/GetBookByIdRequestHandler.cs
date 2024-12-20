using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBookByIdRequestHandler : IRequestHandler<GetBookByIdRequest, ResponseData>
    {
        private IMapper _mapper;
        public IUnitOfWork _unitOfWork;

        public GetBookByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> Handle(GetBookByIdRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var found_book = await _unitOfWork.BookRepository.GetByIdAsync(request.id);
                if (found_book != null)
                {
                    var book = _mapper.Map<BookDTO>(found_book);

                    responseData.Result = book;
                    responseData.IsSuccess = true;
                    responseData.Message = "Book found successfully.";
                }
                else
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Book with this id doesn't exist.";
                }

            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error finding book: {ex.Message}";
            }
            return responseData;
        }
    }
}
