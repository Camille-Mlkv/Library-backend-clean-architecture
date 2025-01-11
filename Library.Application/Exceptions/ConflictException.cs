namespace Library.Application.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message, string? details = null) : base(message, details) { }
    }
}
