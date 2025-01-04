using Library.Application.AuthorUseCases.Queries;
using Library.Application.Exceptions;

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
            var found_author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
            if (found_author is null)
            {
                throw new NotFoundException($"Author with id {request.id} doesn't exist.");
            }
            var author = _mapper.Map<AuthorDTO>(found_author);

            responseData.Result = author;
            responseData.IsSuccess = true;
            responseData.Message = "Author found successfully.";
            return responseData;
        }
    }
}
