using Library.Application.AuthorUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.AuthorUseCases.Commands
{
    public class DeleteAuthorRequestHandler : IRequestHandler<DeleteAuthorRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<object>> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var response=new ResponseData<object>();
            var author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id,cancellationToken);
            if (author is null)
            {
                throw new NotFoundException($"Author with id {request.id} dosen't exist.");
            }

            var books = await _unitOfWork.BookRepository.ListAsync(b => b.AuthorId == request.id,cancellationToken);
            if (books.Any())
            {
                throw new ConflictException("Delete operation failed.", $"Author with id {request.id} can't be deleted as it has associated books.");
            }

            await _unitOfWork.AuthorRepository.Delete(author);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            response.IsSuccess = true;
            response.Message = "Author deleted successfully.";
            return response;
        }
    }
}
