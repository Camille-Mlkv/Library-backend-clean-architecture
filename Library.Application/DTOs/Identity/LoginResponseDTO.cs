namespace Library.Application.DTOs.Identity
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; } //modification
        public string RefreshToken { get; set; }

    }
}
