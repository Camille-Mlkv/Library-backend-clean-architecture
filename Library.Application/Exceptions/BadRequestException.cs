namespace Library.Application.Exceptions
{
    public class BadRequestException:Exception
    {
        public string? Details { get; }
        public BadRequestException(string message, string? details = null) : base(message)
        {
            Details = details;
        }
    }
}
