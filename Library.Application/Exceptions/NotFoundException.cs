namespace Library.Application.Exceptions
{
    public class NotFoundException:Exception
    {
        public string? Details { get; }
        public NotFoundException(string message, string? details = null) : base(message)
        {
            Details = details;
        }
    }
}
