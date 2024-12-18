﻿using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthorUseCases.Queries
{
    public sealed record AddAuthorRequest(AuthorDTO authorDto):IRequest<ResponseData>
    {
    }
}
