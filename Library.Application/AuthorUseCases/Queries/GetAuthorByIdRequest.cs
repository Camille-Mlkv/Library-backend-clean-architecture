using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record GetAuthorByIdRequest(int  id):IRequest<ResponseData>
    {
    }
}
