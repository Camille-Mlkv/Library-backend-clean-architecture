using Library.Application.AuthorUseCases.Queries;


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
            try
            {
                var author = _mapper.Map<Author>(request.author);
                author.Id = request.id;
                await _unitOfWork.AuthorRepository.UpdateAsync(author,cancellationToken);
                await _unitOfWork.SaveAllAsync();
                responseData.IsSuccess = true;
                responseData.Message = "Author updated successfully.";
                responseData.StatusCode = 204;
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error updating author: {ex.Message}";
                responseData.StatusCode = 500;
            }
            return responseData;
        }
    }
}
