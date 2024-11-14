using AutoMapper;
using Library.Application.AuthorUseCases.Queries;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Commands
{
    public class GetAllAuthorsRequestHandler : IRequestHandler<GetAllAuthorsRequest, ResponseData>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetAllAuthorsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(GetAllAuthorsRequest request, CancellationToken cancellationToken)
        {
            var responseData=new ResponseData();
            try
            {
                var authorsList = await _unitOfWork.AuthorRepository.ListAllAsync(cancellationToken);
                var authorDtos=_mapper.Map<List<AuthorDTO>>(authorsList);

                responseData.Result= authorDtos;
                responseData.IsSuccess = true;
                responseData.Message = "All authors are retrieved.";
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
