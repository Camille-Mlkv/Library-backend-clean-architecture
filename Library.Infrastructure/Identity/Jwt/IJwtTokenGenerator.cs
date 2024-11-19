using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Identity.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(ApplicationUser applicationUser, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
