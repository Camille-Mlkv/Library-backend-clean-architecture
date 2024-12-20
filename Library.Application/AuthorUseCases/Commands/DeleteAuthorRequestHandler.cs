using Library.Application.AuthorUseCases.Queries;


namespace Library.Application.AuthorUseCases.Commands
{
    public class DeleteAuthorRequestHandler : IRequestHandler<DeleteAuthorRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var response=new ResponseData();
            try
            {
                var books = await _unitOfWork.BookRepository.ListAsync(b => b.AuthorId == request.id);
                if (books.Any())
                {
                    response.IsSuccess = false;
                    response.Message = "Author can't be deleted as it has associated books.";
                    return response;
                }
                var author=await _unitOfWork.AuthorRepository.GetByIdAsync(request.id);
                if (author!=null)
                {
                    await _unitOfWork.AuthorRepository.DeleteAsync(author);
                    await _unitOfWork.SaveAllAsync();

                    response.IsSuccess = true;
                    response.Message = "Author deleted successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Author with this id doesn't exist.";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while deleting an author:{ex}";
            }
            return response;
        }
    }
}
