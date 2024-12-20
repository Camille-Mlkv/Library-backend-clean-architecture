using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class UpdateBookRequestHandler : IRequestHandler<UpdateBookRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var book = _mapper.Map<Book>(request.NewBook);
                book.Id = request.Id;

                await _unitOfWork.BookRepository.UpdateAsync(book, cancellationToken);
                await _unitOfWork.SaveAllAsync();

                responseData.IsSuccess = true;
                responseData.Message = "Book updated successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error updating book: {ex.Message}";
            }
            return responseData;
        }
    }
}
