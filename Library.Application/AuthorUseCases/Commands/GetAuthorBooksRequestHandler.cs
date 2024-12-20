using Library.Application.AuthorUseCases.Queries;

namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAuthorBooksRequestHandler : IRequestHandler<GetAuthorBooksRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(GetAuthorBooksRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var authorWithBooks=await _unitOfWork.AuthorRepository.ListAsync(a => a.Id == request.id, cancellationToken, a => a.Books);
                if(authorWithBooks != null)
                {
                    responseData.Result = authorWithBooks;
                    responseData.IsSuccess = true;
                    responseData.Message = "Books found for specified author.";
                }
                else
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Author with this id doesn't exist.";
                }
               
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"An error occured: {ex.Message}";
            }
            return responseData;
        }
    }
}
