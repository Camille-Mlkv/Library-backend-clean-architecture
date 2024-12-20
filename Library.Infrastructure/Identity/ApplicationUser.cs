using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
