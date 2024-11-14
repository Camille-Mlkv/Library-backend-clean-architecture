using AutoMapper;
using Library.Application.BookUseCases.Queries;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksPerPageRequestHandler : IRequestHandler<GetBooksPerPageRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksPerPageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(GetBooksPerPageRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                responseData= await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken);
                var books =((ListModel<Book>)responseData.Result).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(books);
                responseData.Result = booksDtos;
                responseData.Message = "Books retrieved successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error: {ex.Message}";
            }
            return responseData;
        }
    }
}
