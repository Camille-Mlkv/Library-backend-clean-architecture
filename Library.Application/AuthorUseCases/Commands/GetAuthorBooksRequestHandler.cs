using Library.Application.AuthorUseCases.Queries;

namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAuthorBooksRequestHandler : IRequestHandler<GetAuthorBooksRequest, ResponseData<Author>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<Author>> Handle(GetAuthorBooksRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<Author>();
            try
            {
                var authorWithBooks=await _unitOfWork.AuthorRepository.ListAsync(a => a.Id == request.id, cancellationToken, a => a.Books);
                if(authorWithBooks is null)
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Author with this id doesn't exist.";
                    responseData.StatusCode = 404;
                    return responseData;
                }
                responseData.Result = authorWithBooks.First();
                responseData.IsSuccess = true;
                responseData.Message = "Books found for specified author.";
                responseData.StatusCode = 200;

            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"An error occured: {ex.Message}";
                responseData.StatusCode = 500;
            }
            return responseData;
        }
    }
}
