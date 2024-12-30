using Library.Application.AuthorUseCases.Queries;


namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAuthorByIdRequestHandler:IRequestHandler<GetAuthorByIdRequest,ResponseData<AuthorDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<AuthorDTO>> Handle(GetAuthorByIdRequest request, CancellationToken cancellationToken)
        {
            var responseData=new ResponseData<AuthorDTO>();
            try
            {
                var found_author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
                if (found_author is null)
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Author with this id doesn't exist.";
                    responseData.StatusCode = 404;
                    return responseData;

                }
                var author = _mapper.Map<AuthorDTO>(found_author);

                responseData.Result = author;
                responseData.IsSuccess = true;
                responseData.Message = "Author found successfully.";
                responseData.StatusCode = 200;

            }
            catch(Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error finding author: {ex.Message}";
                responseData.StatusCode = 500;
            }
            return responseData;
        }
    }
}
