using Library.Application.BookUseCases.Queries;
using Microsoft.AspNetCore.Http;

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
            try
            {
                // check if book with this id exists
                var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
                if(existingBook is null)
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Book with this id doesn't exist.";
                    responseData.StatusCode = 404;
                    return responseData;
                }

                if(existingBook.ISBN!=updatedBook.ISBN)
                {
                    var bookWithIsbn = await _unitOfWork.BookRepository.ListAsync(b => b.ISBN == updatedBook.ISBN);
                    if (bookWithIsbn.Any())
                    {
                        responseData.IsSuccess = false;
                        responseData.Message = "Book with this ISBN already exists.";
                        responseData.StatusCode = 409;
                        return responseData;
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

                await _unitOfWork.BookRepository.UpdateAsync(book, cancellationToken);
                await _unitOfWork.SaveAllAsync();

                responseData.IsSuccess = true;
                responseData.Message = "Book updated successfully.";
                responseData.StatusCode = 204;
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error updating book: {ex.Message}";
                responseData.StatusCode = 500;
            }
            return responseData;
        }
    }
}
