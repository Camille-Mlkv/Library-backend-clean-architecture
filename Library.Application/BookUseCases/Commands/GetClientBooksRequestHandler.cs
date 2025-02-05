﻿using Library.Application.BookUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.BookUseCases.Commands
{
    public class GetClientBooksRequestHandler : IRequestHandler<GetClientBooksRequest, ResponseData<List<BookDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientBooksRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseData<List<BookDTO>>> Handle(GetClientBooksRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<List<BookDTO>>();
            var user = await _unitOfWork.UserRepository.GetUserById(request.CurrentUserId);
            if (user is null)
            {
                throw new UnauthorizedException("Current user not identified.");
            }

            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            if (!roles.Contains("ADMIN"))
            {
                if (request.CurrentUserId != request.ClientId)
                {
                    throw new ConflictException("Failed to load books.","Client can view only his own books.");
                }
            }

            var clientBooks=await _unitOfWork.BookRepository.ListAsync(b=>b.ClientId==request.ClientId, cancellationToken);
            var booksDtos = _mapper.Map<List<BookDTO>>(clientBooks);
            response.Result = booksDtos;
            response.Message = $"Books are retrieved successfully.";
            response.IsSuccess = true;
            return response;
        }
    }
}
