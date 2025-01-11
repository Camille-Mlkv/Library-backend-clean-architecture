using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class UpdateBookRequestHandler : IRequestHandler<UpdateBookRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ResponseData<object>> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<object>();
            var updatedBook=request.UpdatedBook;
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(request.Id, cancellationToken);
            if(existingBook is null)
            {
                throw new NotFoundException($"Book with id {request.Id} doesn't exist.");
            }

            if(existingBook.ISBN!=updatedBook.ISBN)
            {
                var bookWithIsbn = await _unitOfWork.BookRepository.ListAsync(b => b.ISBN == updatedBook.ISBN, cancellationToken);
                if (bookWithIsbn.Any())
                {
                    throw new ConflictException("Failed to update book.", $"Book with ISBN {updatedBook.ISBN} already exists.");
                }
            }

            // image
            if (updatedBook.ImageFile != null)
            {
                if (existingBook.ImagePath != null)
                {
                    _fileService.DeleteFileAsync(existingBook.ImagePath);
                }

                using (var memoryStream = new MemoryStream())
                {
                    await updatedBook.ImageFile.CopyToAsync(memoryStream);
                    updatedBook.ImagePath = await _fileService.SaveFileAsync(memoryStream.ToArray(), updatedBook.ImageFile.FileName);
                }
            }
            else
            {
                updatedBook.ImagePath = "default-book.png";
            }

            var book = _mapper.Map<Book>(updatedBook);
            book.Id = request.Id;

            await _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            responseData.IsSuccess = true;
            responseData.Message = "Book updated successfully.";
            
            return responseData;
        }
    }
}
