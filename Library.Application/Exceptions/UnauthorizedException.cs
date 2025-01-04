namespace Library.Application.Exceptions
{
    public class UnauthorizedException:Exception
    {
        public string? Details { get; }
        public UnauthorizedException(string message, string? details = null) : base(message)
        {
            Details = details;
        }
    }
}
