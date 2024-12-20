using Library.Application.BookUseCases.Queries;

namespace Library.Application.BookUseCases.Commands
{
    public class GetClientBooksRequestHandler : IRequestHandler<GetClientBooksRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(GetClientBooksRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                var clientBooks=await _unitOfWork.BookRepository.ListAsync(b=>b.ClientId==request.ClientId, cancellationToken);
                var booksDtos = _mapper.Map<List<BookDTO>>(clientBooks);
                response.Result = booksDtos;
                response.Message = $"Books are retrieved successfully.";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while retrieving books:{ex}";
            }

            return response;
        }
    }
}
