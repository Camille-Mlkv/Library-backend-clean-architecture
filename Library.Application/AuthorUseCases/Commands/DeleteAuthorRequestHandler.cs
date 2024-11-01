using AutoMapper;
using Library.Application.AuthorUseCases.Queries;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var is_success=await _unitOfWork.AuthorRepository.DeleteAsync(request.id);
                if (is_success)
                {
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
