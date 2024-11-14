using AutoMapper;
using Library.Application.BookUseCases.Queries;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Commands
{
    public class AssignBookToClientRequestHandler : IRequestHandler<AssignBookToClientRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignBookToClientRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData> Handle(AssignBookToClientRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var found_book = await _unitOfWork.BookRepository.GetByIdAsync(request.BookId);
                if (found_book != null)
                {
                    found_book.ClientId = request.ClientId;
                    found_book.TakenTime= DateTime.UtcNow;
                    found_book.ReturnBy= DateTime.UtcNow.AddDays(14);

                    await _unitOfWork.BookRepository.UpdateAsync(found_book, cancellationToken);
                    responseData.IsSuccess = true;
                    responseData.Message = $"Book assigned to client {request.ClientId}.";
                }
                else
                {
                    responseData.IsSuccess = false;
                    responseData.Message = "Book with this id doesn't exist.";
                }
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error assigning book: {ex.Message}";
            }
            return responseData;
        }
    }
}
