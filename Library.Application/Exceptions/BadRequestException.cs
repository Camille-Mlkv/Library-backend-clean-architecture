namespace Library.Application.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, string? details = null) : base(message, details) { }
    }
}
