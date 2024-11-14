using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetBooksByIsbnRequest(string Isbn,int PageNo,int PageSize):IRequest<ResponseData>
    {
    }
}
