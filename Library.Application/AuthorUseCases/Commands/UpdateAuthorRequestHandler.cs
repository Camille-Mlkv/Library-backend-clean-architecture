using Library.Application.AuthorUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.AuthorUseCases.Commands
{
    public class UpdateAuthorRequestHandler : IRequestHandler<UpdateAuthorRequest, ResponseData<AuthorDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<AuthorDTO>> Handle(UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<AuthorDTO>();
            var found_author = await _unitOfWork.AuthorRepository.GetByIdAsync(request.id, cancellationToken);
            if (found_author is null)
            {
                throw new NotFoundException($"Author with id {request.id} doesn't exist.");
            }

            var author = _mapper.Map<Author>(request.author);
            author.Id = request.id;
            await _unitOfWork.AuthorRepository.Update(author);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            responseData.IsSuccess = true;
            responseData.Message = "Author updated successfully.";
            return responseData;
        }
    }
}
