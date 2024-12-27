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
                var user = await _unitOfWork.UserRepository.GetUserById(request.CurrentUserId);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Current user not identified";
                    return response;
                }

                var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
                if (!roles.Contains("ADMIN"))
                {
                    if (request.CurrentUserId != request.ClientId)
                    {
                        response.IsSuccess = false;
                        response.Message = $"Client can view only his own books";
                        return response;
                    }
                }

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
