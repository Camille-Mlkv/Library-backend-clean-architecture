using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBookByIdRequestHandler : IRequestHandler<GetBookByIdRequest, ResponseData<BookDTO>>
    {
        private IMapper _mapper;
        public IUnitOfWork _unitOfWork;

        public GetBookByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<BookDTO>> Handle(GetBookByIdRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<BookDTO>();

            var found_book = await _unitOfWork.BookRepository.GetByIdAsync(request.id, cancellationToken);
            if (found_book is null)
            {
                throw new NotFoundException($"Book with id {request.id} doesn't exist.");
            }

            var book = _mapper.Map<BookDTO>(found_book);
            responseData.Result = book;
            responseData.IsSuccess = true;
            responseData.Message = "Book found successfully.";
            return responseData;
        }
    }
}
