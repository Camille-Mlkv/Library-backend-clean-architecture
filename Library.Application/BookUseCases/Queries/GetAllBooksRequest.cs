﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.BookUseCases.Queries
{
    public sealed record GetAllBooksRequest:IRequest<ResponseData>
    {
    }
}
