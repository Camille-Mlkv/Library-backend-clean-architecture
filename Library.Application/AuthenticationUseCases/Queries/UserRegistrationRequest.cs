using Library.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.AuthenticationUseCases.Queries
{
    public sealed record UserRegistrationRequest(RegistrationRequestDTO RegistrationRequest):IRequest<ResponseData>
    {
    }
}
