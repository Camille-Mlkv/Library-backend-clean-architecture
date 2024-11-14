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
    public class GetBooksByAuthorRequestHandler : IRequestHandler<GetBooksByAuthorRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(GetBooksByAuthorRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData();
            try
            {
                response = await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken, b => b.AuthorId == request.AuthorId);
                var books = ((ListModel<Book>)response.Result).Items;
                var booksDtos = _mapper.Map<List<BookDTO>>(books);
                response.Result = booksDtos;
                response.Message = $"Books are retrieved successfully.";
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
