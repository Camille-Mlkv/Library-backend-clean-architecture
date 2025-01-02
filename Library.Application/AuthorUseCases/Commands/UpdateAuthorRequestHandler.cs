using Library.Application.AuthorUseCases.Queries;
using Library.Application.Utilities;


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
            var found_author = await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
            if (found_author is null)
            {
                throw new CustomHttpException(404, "Not found.", $"Author with id {request.id} doesn't exist.");
            }

            var author = _mapper.Map<Author>(request.author);
            author.Id = request.id;
            await _unitOfWork.AuthorRepository.UpdateAsync(author,cancellationToken);
            await _unitOfWork.SaveAllAsync();

            responseData.IsSuccess = true;
            responseData.Message = "Author updated successfully.";
            return responseData;
        }
    }
}
