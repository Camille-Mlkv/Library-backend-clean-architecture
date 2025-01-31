﻿using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.Exceptions;

namespace Library.Application.AuthenticationUseCases.Commands
{
    public class RevokeRefreshTokenRequestHandler : IRequestHandler<RevokeRefreshTokenRequest, ResponseData<object>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeRefreshTokenRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<object>> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseData<object>();
            var user = await _unitOfWork.UserRepository.GetUserByUsername(request.Username);
            if (user is null)
            {
                throw new NotFoundException("Error while revoking refresh token.",$"User with username {request.Username} not found.");
            }
            user.RefreshToken = null;
            await _unitOfWork.UserRepository.UpdateUserTokens(user);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            response.IsSuccess = true;
            response.Message = "Refresh token revoked";
            return response;
        }
    }
}
