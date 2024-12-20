using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class AddBookRequestHandler : IRequestHandler<AddBookRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var book = _mapper.Map<Book>(request.book);
                await _unitOfWork.BookRepository.AddAsync(book);
                await _unitOfWork.SaveAllAsync();

                var createdBook = _mapper.Map<BookDTO>(book);
                responseData.Result = createdBook;
                responseData.IsSuccess = true;
                responseData.Message = "Book added successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error adding book: {ex.Message}";
            }

            return responseData;
        }
    }
}
