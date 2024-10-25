using Library.Application.AuthorUseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Commands
{
    public class AddAuthorRequestHandler : IRequestHandler<AddAuthorRequest, Author>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddAuthorRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Author> Handle(AddAuthorRequest request, CancellationToken cancellationToken)
        {
            Author newAuthor = new Author();
            await _unitOfWork.AuthorRepository.AddAsync(newAuthor, cancellationToken);
            return newAuthor;
        }
    }
}
