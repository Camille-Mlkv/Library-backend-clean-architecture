namespace Library.Application.DTOs.Identity
{
    public class RefreshModelDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
