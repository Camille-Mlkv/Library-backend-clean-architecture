using AutoMapper;
using Library.Application.AuthorUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Commands
{
    public class UpdateAuthorRequestHandler : IRequestHandler<UpdateAuthorRequest, ResponseData>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAuthorRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData> Handle(UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            var responseData = new ResponseData();
            try
            {
                var author = _mapper.Map<Author>(request.author);
                author.Id = request.id;
                await _unitOfWork.AuthorRepository.UpdateAsync(author,cancellationToken);

                //responseData.Result = author;
                responseData.IsSuccess = true;
                responseData.Message = "Author updated successfully.";
            }
            catch (Exception ex)
            {
                responseData.IsSuccess = false;
                responseData.Message = $"Error updating author: {ex.Message}";
            }
            return responseData;
        }
    }
}
