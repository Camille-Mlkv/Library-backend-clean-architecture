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
    public class GetAllBooksRequestHandler : IRequestHandler<GetAllBooksRequest, ResponseData>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetAllBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(GetAllBooksRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var booksList = await _unitOfWork.BookRepository.ListAllAsync(cancellationToken);
                var booksDtos = _mapper.Map<List<BookDTO>>(booksList);

                responseData.Result = booksDtos;
                responseData.IsSuccess = true;
                responseData.Message = "All books are retrieved.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"An error occured:{ex.Message}.";
            }
            return responseData;
        }
    }
}
