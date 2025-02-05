﻿using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetBooksPerPageRequestHandler : IRequestHandler<GetBooksPerPageRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksPerPageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<List<BookDTO>>> Handle(GetBooksPerPageRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData<List<BookDTO>>();
            if(request.PageNo<1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var bookList=await _unitOfWork.BookRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken);
            var booksDtos = _mapper.Map<List<BookDTO>>(bookList);
            responseData.Result = booksDtos;
            responseData.Message = "Books retrieved successfully.";
            responseData.IsSuccess=true;
            return responseData;
        }
    }
}
