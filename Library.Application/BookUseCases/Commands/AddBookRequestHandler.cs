using AutoMapper;
using Library.Application.BookUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Commands
{
    public class AddBookRequestHandler : IRequestHandler<AddBookRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddBookRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var book = _mapper.Map<Book>(request.book);
                await _unitOfWork.BookRepository.AddAsync(book);

                responseData.Result = book;//replace with dto
                responseData.IsSuccess = true;
                responseData.Message = "Book added successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error adding book: {ex.Message}";
            }

            return responseData;
        }
    }
}
