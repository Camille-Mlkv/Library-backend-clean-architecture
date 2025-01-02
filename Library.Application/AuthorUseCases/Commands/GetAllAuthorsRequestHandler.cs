using Library.Application.AuthorUseCases.Queries;

namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAllAuthorsRequestHandler : IRequestHandler<GetAllAuthorsRequest, ResponseData<List<AuthorDTO>>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetAllAuthorsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<AuthorDTO>>> Handle(GetAllAuthorsRequest request, CancellationToken cancellationToken)
        {
            var responseData=new ResponseData<List<AuthorDTO>>();
            var authorsList = await _unitOfWork.AuthorRepository.ListAllAsync(cancellationToken);
            var authorDtos=_mapper.Map<List<AuthorDTO>>(authorsList);

            responseData.Result= authorDtos;
            responseData.IsSuccess = true;
            responseData.Message = "All authors are retrieved.";
            return responseData;
        }
    }
}
