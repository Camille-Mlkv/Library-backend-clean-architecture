using Library.Application.AuthorUseCases.Queries;


namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAuthorByIdRequestHandler:IRequestHandler<GetAuthorByIdRequest,ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(GetAuthorByIdRequest request, CancellationToken cancellationToken)
        {
            var responseData=new ResponseData();
            try
            {
                var found_author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
                if (found_author != null)
                {
                    var author = _mapper.Map<AuthorDTO>(found_author);

                    responseData.Result = author;
                    responseData.IsSuccess = true;
                    responseData.Message = "Author found successfully.";
                }
                else
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Author with this id doesn't exist.";
                }
                
            }
            catch(Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error finding author: {ex.Message}";
            }
            return responseData;
        }
    }
}
