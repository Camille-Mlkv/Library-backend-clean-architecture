﻿using Library.Application.BookUseCases.Queries;
using Library.Application.Utilities;

namespace Library.Application.BookUseCases.Commands
{
    public class DeleteBookRequestHandler : IRequestHandler<DeleteBookRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public DeleteBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ResponseData<object>> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<object>();
            var book =await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            if(book is null)
            {
                throw new CustomHttpException(404, "Not found.", $"Book with id {request.Id} doesn't exist.");
            }

            if (!string.IsNullOrEmpty(book.ImagePath) && book.ImagePath != "default-book.png")
            {
                _fileService.DeleteFileAsync(book.ImagePath);
            }

            await _unitOfWork.BookRepository.DeleteAsync(book);
            await _unitOfWork.SaveAllAsync();

            response.IsSuccess = true;
            response.Message = "Book deleted successfully.";
            return response;
        }
    }
}
