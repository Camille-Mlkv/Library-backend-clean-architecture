using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class AddBookRequestHandler : IRequestHandler<AddBookRequest, ResponseData<BookDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AddBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ResponseData<BookDTO>> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<BookDTO>();

            var formFile=request.book.ImageFile;
            var bytes = new byte[0];
            var filename = "";

            // image
            if(formFile != null && formFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                    filename=formFile.FileName;
                }
            }

            request.book.ImagePath=await _fileService.SaveFileAsync(bytes, filename);

            var book = _mapper.Map<Book>(request.book);
            var bookWithIsbn=await _unitOfWork.BookRepository.ListAsync(b=>b.ISBN==request.book.ISBN);
            if(bookWithIsbn.Any())
            {
                throw new ConflictException("Resource already exists.", $"Book with ISBN {request.book.ISBN} already exists.");
            }

            await _unitOfWork.BookRepository.AddAsync(book);
            await _unitOfWork.SaveAllAsync();

            var createdBook = _mapper.Map<BookDTO>(book);
            responseData.Result = createdBook;
            responseData.IsSuccess = true;
            responseData.Message = "Book added successfully.";
            return responseData;
        }
    }
}
