using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBookImageRequestHandler :IRequestHandler<GetBookImageRequest, byte[]>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookImageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<byte[]> Handle(GetBookImageRequest request, CancellationToken cancellationToken)
        {
            var book=await _unitOfWork.BookRepository.GetByIdAsync(request.BookId);
            if(book == null)
            {
                return null;
            }
            var imagePath = Path.Combine("wwwroot", "Images", $"{book.ImagePath}");
            var fileBytes = await File.ReadAllBytesAsync(imagePath);
            return fileBytes;
        }
    }
}
