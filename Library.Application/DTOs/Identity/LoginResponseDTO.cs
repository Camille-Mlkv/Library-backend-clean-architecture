namespace Library.Application.DTOs.Identity
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }

    }
}
