using AutoMapper;
using Library.Application.BookUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Commands
{
    public class DeleteBookRequestHandler : IRequestHandler<DeleteBookRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                var is_success = await _unitOfWork.BookRepository.DeleteAsync(request.Id);
                if (is_success)
                {
                    response.IsSuccess = true;
                    response.Message = "Book deleted successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Book with this id doesn't exist.";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occured while deleting a book:{ex}";
            }
            return response;
        }
    }
}
