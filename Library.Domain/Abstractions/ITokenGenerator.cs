using Library.Domain.Entities;
using System.Security.Claims;

namespace Library.Domain.Abstractions
{
    public interface ITokenGenerator
    {
        (string AccessToken, DateTime Expiry) GenerateAccessToken(User user, IEnumerable<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}
