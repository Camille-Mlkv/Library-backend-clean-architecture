using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class AssignBookToClientRequestHandler : IRequestHandler<AssignBookToClientRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignBookToClientRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<object>> Handle(AssignBookToClientRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<object>();
            var found_book = await _unitOfWork.BookRepository.GetByIdAsync(request.BookId, cancellationToken);
            if(found_book is null)
            {
                throw new NotFoundException($"Book with id {request.BookId} doesn't exist.");
            }

            if (found_book.ClientId != null)
            {
                throw new ConflictException("Assigning failed.", "Cannot assign the book as it's already assigned.");
            }

            var existingClient = await _unitOfWork.UserRepository.GetUserById(request.ClientId);
            if (existingClient is null)
            {
                throw new NotFoundException($"User with id {request.ClientId} doesn't exist.");
            }

            found_book.ClientId = request.ClientId;
            found_book.TakenTime= DateTime.UtcNow;
            found_book.ReturnBy= DateTime.UtcNow.AddDays(14);

            await _unitOfWork.BookRepository.UpdateAsync(found_book, cancellationToken);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            responseData.IsSuccess = true;
            responseData.Message = $"Book assigned to client {request.ClientId}.";
            return responseData;
        }
    }
}
