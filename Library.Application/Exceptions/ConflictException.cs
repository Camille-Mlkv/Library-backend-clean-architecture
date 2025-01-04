namespace Library.Application.Exceptions
{
    public class ConflictException:Exception
    {
        public string? Details { get; }
        public ConflictException(string message, string? details = null) : base(message)
        {
            Details = details;
        }
    }
}
