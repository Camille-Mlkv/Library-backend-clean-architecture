using Library.Application.AuthorUseCases.Queries;


namespace Library.Application.AuthorUseCases.Commands
{
    public class DeleteAuthorRequestHandler : IRequestHandler<DeleteAuthorRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<object>> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var response=new ResponseData<object>();
            try
            {
                var books = await _unitOfWork.BookRepository.ListAsync(b => b.AuthorId == request.id);
                if (books.Any())
                {
                    response.IsSuccess = false;
                    response.Message = "Author can't be deleted as it has associated books.";
                    response.StatusCode = 409;
                    return response;
                }
                var author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
                if (author is null)
                {
                    response.IsSuccess = false;
                    response.Message = "Author with this id doesn't exist.";
                    response.StatusCode = 404;
                    return response;
                }

                await _unitOfWork.AuthorRepository.DeleteAsync(author);
                await _unitOfWork.SaveAllAsync();

                response.IsSuccess = true;
                response.Message = "Author deleted successfully.";
                response.StatusCode = 204;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while deleting an author:{ex}";
                response.StatusCode = 500;
            }
            return response;
        }
    }
}
