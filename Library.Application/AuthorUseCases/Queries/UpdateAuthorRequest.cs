using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record UpdateAuthorRequest(int id, AuthorDTO author):IRequest<ResponseData>
    {
    }
}
