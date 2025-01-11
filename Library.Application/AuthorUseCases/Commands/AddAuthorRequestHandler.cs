using Library.Application.AuthorUseCases.Queries;


namespace Library.Application.AuthorUseCases.Commands
{
    public class AddAuthorRequestHandler : IRequestHandler<AddAuthorRequest, ResponseData<AuthorDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddAuthorRequestHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }
        public async Task<ResponseData<AuthorDTO>> Handle(AddAuthorRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<AuthorDTO>();
            var author = _mapper.Map<Author>(request.authorDto);
            await _unitOfWork.AuthorRepository.AddAsync(author, cancellationToken);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            var authorDto=_mapper.Map<AuthorDTO>(author);

            responseData.Result = authorDto;
            responseData.IsSuccess = true;
            responseData.Message = "Author added successfully.";
      
            return responseData;

        }
    }
}
