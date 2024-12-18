﻿using AutoMapper;
using Library.Application.AuthorUseCases.Queries;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Commands
{
    public class AddAuthorRequestHandler : IRequestHandler<AddAuthorRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddAuthorRequestHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }
        public async Task<ResponseData> Handle(AddAuthorRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var author = _mapper.Map<Author>(request.authorDto);
                await _unitOfWork.AuthorRepository.AddAsync(author, cancellationToken);

                var authorDto=_mapper.Map<AuthorDTO>(author);

                responseData.Result = authorDto;
                responseData.IsSuccess = true;
                responseData.Message = "Author added successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error adding author: {ex.Message}";
            }

            return responseData;

        }
    }
}
